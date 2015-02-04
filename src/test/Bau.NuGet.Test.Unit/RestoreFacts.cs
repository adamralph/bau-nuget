// <copyright file="RestoreFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.IO;
    using System.Linq;
    using System.Threading;
    using FluentAssertions;
    using Xunit;

    public static class RestoreFacts
    {
        [Fact]
        public static void Restores()
        {
            // arrange
            var restore = new Restore()
                .Files("./restore-test/packages.config")
                .In("./")
                .SolutionIn("./restore-test")
                .PackagesIn("./restore-test/packages")
                .RequiresConsent(false);

            if (!Directory.Exists(restore.SolutionDirectory))
            {
                Directory.CreateDirectory(restore.SolutionDirectory);
            }

            if (Directory.Exists(restore.PackagesDirectory))
            {
                Thread.Sleep(100);
                Directory.Delete(restore.PackagesDirectory, true);
                Thread.Sleep(100);
            }

            using (var packagesFileStream = File.CreateText(restore.Files.Single()))
            {
                packagesFileStream.Write(
                    "<packages><package id=\"Bau\" version=\"0.1.0-beta01\" targetFramework=\"net45\" /></packages>");
            }

            Directory.Exists(restore.PackagesDirectory).Should().BeFalse();
            File.Exists(Path.Combine(restore.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll"))
                .Should().BeFalse();

            // act
            restore.Execute();

            // assert
            Directory.Exists(restore.PackagesDirectory).Should().BeTrue();
            File.Exists(Path.Combine(restore.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll"))
                .Should().BeTrue();
        }

        [Fact]
        public static void CreatesMultipleRestoreCommands()
        {
            // arrange
            var task = new Restore();
            var fakeDirName = "./fake-dir/";

            // act
            task
                .Files("file1", "file2")
                .In(fakeDirName)
                .PackagesIn(fakeDirName);

            // assert
            task.Files.Should().HaveCount(2);
            task.WorkingDirectory.Should().Be(fakeDirName);
            task.PackagesDirectory.Should().Be(fakeDirName);
            task.Files.Should().Contain("file1");
            task.Files.Should().Contain("file2");
        }

        [Fact]
        public static void PropertySource()
        {
            // arrange
            var normal = new Restore();
            var multiple = new Restore();
            multiple.Sources.Add(@"http://source1/api");
            multiple.Sources.Add(@"C:\some folder\");

            // act
            var normalArguments = normal.CreateCommandLineOptions();
            var multipleArguments = multiple.CreateCommandLineOptions();

            // assert
            normalArguments.Should().NotContain("-Source");
            multipleArguments.Should().Contain(@"-Source http://source1/api");
            multipleArguments.Should().Contain(@"-Source ""C:\some folder/""");
        }

        [Fact]
        public static void PropertySourceFluent()
        {
            // arrange
            var normal = new Restore();
            var multiple = new Restore();

            // act
            multiple
                .UseSource(@"http://source1/api")
                .UseSource(@"C:\some folder\");

            // assert
            normal.Sources.Should().BeEmpty();
            multiple.Sources.Should().Equal(new[] { @"http://source1/api", @"C:\some folder\" });
        }

        [Fact]
        public static void PropertyNoCache()
        {
            // arrange
            var normal = new Restore();
            var enabled = new Restore { NoCache = true };
            var disabled = new Restore { NoCache = false };

            // act
            var normalArguments = normal.CreateCommandLineOptions();
            var enabledArguments = enabled.CreateCommandLineOptions();
            var disabledArguments = disabled.CreateCommandLineOptions();

            // assert
            normalArguments.Should().NotContain("-NoCache");
            enabledArguments.Should().Contain("-NoCache");
            disabledArguments.Should().NotContain("-NoCache");
        }

        [Fact]
        public static void PropertyNoCacheFluent()
        {
            // arrange
            var normal = new Restore();
            var enabled = new Restore();
            var disabled = new Restore();

            // act
            normal.DisableCache();
            enabled.DisableCache(true);
            disabled.DisableCache(false);

            // assert
            normal.NoCache.Should().BeTrue();
            enabled.NoCache.Should().BeTrue();
            disabled.NoCache.Should().BeFalse();
        }

        [Fact]
        public static void PropertyDisableParallelProcessing()
        {
            // arrange
            var normal = new Restore();
            var enabled = new Restore { ParallelProcessingDisabled = true };
            var disabled = new Restore { ParallelProcessingDisabled = false };

            // act
            var normalArguments = normal.CreateCommandLineOptions();
            var enabledArguments = enabled.CreateCommandLineOptions();
            var disabledArguments = disabled.CreateCommandLineOptions();

            // assert
            normalArguments.Should().NotContain("-DisableParallelProcessing");
            enabledArguments.Should().Contain("-DisableParallelProcessing");
            disabledArguments.Should().NotContain("-DisableParallelProcessing");
        }

        [Fact]
        public static void PropertyDisableParallelProcessingFluent()
        {
            // arrange
            var normal = new Restore();
            var enabled = new Restore();
            var disabled = new Restore();

            // act
            normal.DisableParallelProcessing();
            enabled.DisableParallelProcessing(true);
            disabled.DisableParallelProcessing(false);

            // assert
            normal.ParallelProcessingDisabled.Should().BeTrue();
            enabled.ParallelProcessingDisabled.Should().BeTrue();
            disabled.ParallelProcessingDisabled.Should().BeFalse();
        }

        [Fact]
        public static void CanHaveExtraArgsAdded()
        {
            // arrange
            var extraArg = "-DoMagicThings";
            var task = new Restore().With(new[] { extraArg });

            // act
            var arguments = task.CreateCommandLineOptions();

            // assert
            arguments.Should().Contain(extraArg);
            task.ExtraArgs.Should().BeEquivalentTo(extraArg);
        }
    }
}

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
        public static void CanRestorePackagesUsingCli()
        {
            // arrange
            var task = new NuGetTask();
            var restore = task
                .Restore("./restore-test/packages.config")
                .WithWorkingDirectory("./")
                .WithSolutionDirectory("./restore-test")
                .WithPackagesDirectory("./restore-test/packages")
                .WithRequiresConsent(false);

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

            using (var packagesFileStream = File.CreateText(restore.SolutionOrPackagesConfig))
            {
                packagesFileStream.Write(
                    "<packages><package id=\"Bau\" version=\"0.1.0-beta01\" targetFramework=\"net45\" /></packages>");
            }

            Directory.Exists(restore.PackagesDirectory).Should().BeFalse();
            File.Exists(Path.Combine(restore.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll"))
                .Should().BeFalse();

            // act
            task.Execute();

            // assert
            Directory.Exists(restore.PackagesDirectory).Should().BeTrue();
            File.Exists(Path.Combine(restore.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll"))
                .Should().BeTrue();
        }

        [Fact]
        public static void CanCreateMultipleRestoreCommands()
        {
            // arrange
            var task = new NuGetTask();
            var fakeDirName = "./fake-dir/";

            // act
            task.Restore(
                new[] { "file1", "file2" },
                r => r
                    .WithWorkingDirectory(fakeDirName)
                    .WithPackagesDirectory(fakeDirName));

            // assert
            task.Commands.Should().HaveCount(2);
            task.Commands.All(r => r.WorkingDirectory == fakeDirName).Should().BeTrue();
            task.Commands.OfType<Restore>().All(r => r.PackagesDirectory == fakeDirName).Should().BeTrue();
            task.Commands.OfType<Restore>().Select(x => x.SolutionOrPackagesConfig).Should().Contain("file1");
            task.Commands.OfType<Restore>().Select(x => x.SolutionOrPackagesConfig).Should().Contain("file2");
        }

        [Fact]
        public static void PropertySourceCli()
        {
            // arrange
            var normal = new Restore();
            var multiple = new Restore();
            multiple.Sources.Add(@"http://source1/api");
            multiple.Sources.Add(@"C:\some folder\");

            // act
            var normalArguments = normal.CreateCommandLineArguments();
            var multipleArguments = multiple.CreateCommandLineArguments();

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
                .WithSource(@"http://source1/api")
                .WithSource(@"C:\some folder\");

            // assert
            normal.Sources.Should().BeEmpty();
            multiple.Sources.Should().Equal(new[] { @"http://source1/api", @"C:\some folder\" });
        }

        [Fact]
        public static void PropertyNoCacheCli()
        {
            // arrange
            var normal = new Restore();
            var enabled = new Restore { NoCache = true };
            var disabled = new Restore { NoCache = false };

            // act
            var normalArguments = normal.CreateCommandLineArguments();
            var enabledArguments = enabled.CreateCommandLineArguments();
            var disabledArguments = disabled.CreateCommandLineArguments();

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
            normal.WithNoCache();
            enabled.WithNoCache(true);
            disabled.WithNoCache(false);

            // assert
            normal.NoCache.Should().BeTrue();
            enabled.NoCache.Should().BeTrue();
            disabled.NoCache.Should().BeFalse();
        }

        [Fact]
        public static void PropertyDisableParallelProcessingCli()
        {
            // arrange
            var normal = new Restore();
            var enabled = new Restore { DisableParallelProcessing = true };
            var disabled = new Restore { DisableParallelProcessing = false };

            // act
            var normalArguments = normal.CreateCommandLineArguments();
            var enabledArguments = enabled.CreateCommandLineArguments();
            var disabledArguments = disabled.CreateCommandLineArguments();

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
            normal.WithDisableParallelProcessing();
            enabled.WithDisableParallelProcessing(true);
            disabled.WithDisableParallelProcessing(false);

            // assert
            normal.DisableParallelProcessing.Should().BeTrue();
            enabled.DisableParallelProcessing.Should().BeTrue();
            disabled.DisableParallelProcessing.Should().BeFalse();
        }
    }
}

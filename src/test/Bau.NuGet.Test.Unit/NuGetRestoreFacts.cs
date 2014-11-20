// <copyright file="NuGetRestoreFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public static class NuGetRestoreFacts
    {        
        [Fact]
        public static void CanRestorePackagesUsingCli()
        {
            // arrange
            var task = new NuGetTask();
            var request = task
                .Restore("./restore-test/packages.config")
                .WithWorkingDirectory("./")
                .WithSolutionDirectory("./restore-test")
                .WithPackagesDirectory("./restore-test/packages")
                .WithRequiresConsent(false);

            NuGetCliLocatorFacts.InstallNuGetCli();

            if (!Directory.Exists(request.SolutionDirectory))
            {
                Directory.CreateDirectory(request.SolutionDirectory);
            }

            if (Directory.Exists(request.PackagesDirectory))
            {
                Thread.Sleep(100);
                Directory.Delete(request.PackagesDirectory, true);
                Thread.Sleep(100);
            }

            using (var packagesFileStream = File.CreateText(request.TargetSolutionOrPackagesConfig))
            {
                packagesFileStream.Write("<packages><package id=\"Bau\" version=\"0.1.0-beta01\" targetFramework=\"net45\" /></packages>");
            }

            Directory.Exists(request.PackagesDirectory).Should().BeFalse();
            File.Exists(Path.Combine(request.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll")).Should().BeFalse();

            // act
            task.Execute();

            // assert
            Directory.Exists(request.PackagesDirectory).Should().BeTrue();
            File.Exists(Path.Combine(request.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll")).Should().BeTrue();
        }

        [Fact]
        public static void CanCreateMultipleRestoreRequests()
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
            task.Requests.Should().HaveCount(2);
            task.Requests.All(r => r.WorkingDirectory == fakeDirName).Should().BeTrue();
            task.Requests.OfType<NuGetCliRestoreCommandRequest>().All(r => r.PackagesDirectory == fakeDirName).Should().BeTrue();
            task.Requests.OfType<NuGetCliRestoreCommandRequest>().Select(x => x.TargetSolutionOrPackagesConfig).Should().Contain("file1");
            task.Requests.OfType<NuGetCliRestoreCommandRequest>().Select(x => x.TargetSolutionOrPackagesConfig).Should().Contain("file2");
        }
    }
}

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
        public static void CanRestorePackages()
        {
            // arrange
            NuGetCliLocatorFacts.InstallNuGetCli();

            var restore = new NuGetRestore()
                .For("./restore-test/packages.config")
                .WithSolutionDirectory("./restore-test")
                .WithPackagesDirectory("./restore-test/packages")
                .InWorkingDirectory("./");

            var request = restore.Request;

            if (!Directory.Exists(request.SolutionDirectory))
            {
                Directory.CreateDirectory(request.SolutionDirectory);
            }

            if (Directory.Exists(request.PackagesDirectory))
            {
                Thread.Sleep(500);
                Directory.Delete(request.PackagesDirectory, true);
                Thread.Sleep(500);
            }

            using (var packagesFile = File.Open(request.TargetSolutionOrPackagesConfig, FileMode.Create))
            using (var packagesFileStream = new StreamWriter(packagesFile))
            {
                packagesFileStream.Write("<packages><package id=\"Bau\" version=\"0.1.0-beta01\" targetFramework=\"net45\" /></packages>");
            }

            Directory.Exists(request.PackagesDirectory).Should().BeFalse();
            File.Exists(Path.Combine(request.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll")).Should().BeFalse();

            // act
            restore.Execute();

            // assert
            Directory.Exists(request.PackagesDirectory).Should().BeTrue();
            File.Exists(Path.Combine(request.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll")).Should().BeTrue();
        }
    }
}

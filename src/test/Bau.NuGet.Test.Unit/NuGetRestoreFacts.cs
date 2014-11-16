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
        public static void CanApplyRestoreCommandParameters()
        {
            // arrange
            NuGetCliLoaderFacts.InstallNuGetCli();
            var loader = new NuGetCliLoader();
            var wrapper = new NuGetCliReflectionWrapper(loader.Assembly);
            var request = new NuGetCliRestoreCommandRequest
            {
                ConfigFile = "poopy.poop",
                NonInteractive = false,
                Verbosity = "detailed",
                DisableParallelProcessing = true,
                NoCache = true,
                PackageSaveMode = "do whatever",
                RequireConsent = true,
                PackagesDirectory = "./",
                SolutionDirectory = "../solution"                
            };
            request.Source.AddRange(new[] { "some-place", "some-other-place" });

            // act
            var command = Activator.CreateInstance(wrapper.RestoreCommandType);
            request.Apply(command);

            // assert
            wrapper.RestoreCommandType.GetProperty("ConfigFile").GetValue(command).Should().Be(request.ConfigFile);
            wrapper.RestoreCommandType.GetProperty("NonInteractive").GetValue(command).Should().Be(request.NonInteractive);
            Assert.Equal(request.Verbosity, wrapper.RestoreCommandType.GetProperty("Verbosity").GetValue(command).ToString(), StringComparer.OrdinalIgnoreCase);
            wrapper.RestoreCommandType.GetProperty("DisableParallelProcessing").GetValue(command).Should().Be(request.DisableParallelProcessing);
            wrapper.RestoreCommandType.GetProperty("NoCache").GetValue(command).Should().Be(request.NoCache);
            wrapper.RestoreCommandType.GetProperty("PackageSaveMode").GetValue(command).Should().Be(request.PackageSaveMode);
            Assert.Equal<string>(request.Source, (ICollection<string>)wrapper.RestoreCommandType.GetProperty("Source").GetValue(command));
            wrapper.RestoreCommandType.GetProperty("RequireConsent").GetValue(command).Should().Be(request.RequireConsent);
            wrapper.RestoreCommandType.GetProperty("PackagesDirectory").GetValue(command).Should().Be(request.PackagesDirectory);
            wrapper.RestoreCommandType.GetProperty("SolutionDirectory").GetValue(command).Should().Be(request.SolutionDirectory);
        }

        [Fact]
        public static void CanRestorePackages()
        {
            // arrange
            NuGetCliLoaderFacts.InstallNuGetCli();
            var loader = new NuGetCliLoader();
            var wrapper = new NuGetCliReflectionWrapper(loader.Assembly);
            var request = new NuGetCliRestoreCommandRequest
            {
                SolutionDirectory = new FileInfo("./restore-test").FullName,
                PackagesDirectory = new FileInfo("./restore-test/packages").FullName
            };

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

            using (var packagesFile = File.Open(Path.Combine(request.SolutionDirectory, "packages.config"), FileMode.Create))
            using (var packagesFileStream = new StreamWriter(packagesFile))
            {
                packagesFileStream.Write("<packages><package id=\"Bau\" version=\"0.1.0-beta01\" targetFramework=\"net45\" /></packages>");
            }

            Directory.Exists(request.PackagesDirectory).Should().BeFalse();
            File.Exists(Path.Combine(request.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll")).Should().BeFalse();

            // act
            wrapper.Restore(request);

            // assert
            Directory.Exists(request.PackagesDirectory).Should().BeTrue();
            File.Exists(Path.Combine(request.PackagesDirectory, "Bau.0.1.0-beta01/lib/net45/Bau.dll")).Should().BeTrue();
        }
    }
}

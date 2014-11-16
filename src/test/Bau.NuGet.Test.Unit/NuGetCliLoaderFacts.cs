// <copyright file="NuGetCliLoaderFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;
    using System.Threading;

    public static class NuGetCliLoaderFacts
    {
        [Fact]
        public static void CanFindYourInnerSelf()
        {
            // arrange
            var expectedPath = typeof(NuGetCliLoader).Assembly.Location; // NOTE: some test runners may have issues with this line
            var loader = new NuGetCliLoader();

            // act
            var actualPath = loader.GetBauNuGetPluginAssemblyPath();

            // assert
            Assert.Equal(expectedPath, actualPath, StringComparer.Ordinal);
        }

        [Fact]
        public static void CanFindNuGetCommandLineExe()
        {
            // arrange
            InstallNuGetCli();
            var loader = new NuGetCliLoader();

            // act
            var actualPath = loader.GetNugetCommandLineAssemblyPath();

            // assert
            actualPath.FullName.Should().EndWith("NuGet.exe");
            actualPath.Exists.Should().BeTrue();
        }

        [Fact]
        public static void CanLoadTwice()
        {
            // arrange
            InstallNuGetCli();
            var loader1 = new NuGetCliLoader();
            var loader2 = new NuGetCliLoader();

            // act
            var asm1 = loader1.Assembly;
            var asm2 = loader2.Assembly;

            // assert
            Assert.Same(asm1, asm2);
        }

        internal static void InstallNuGetCli()
        {
            var currentDirectory = Path.GetDirectoryName(typeof(NuGetCliLoader).Assembly.Location);
            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "scriptcs", // hope it is on the PATH
                Arguments = "-install NuGet.CommandLine",
                UseShellExecute = false,
                WorkingDirectory = currentDirectory
            };
            var installResult = System.Diagnostics.Process.Start(processStartInfo);
            installResult.WaitForExit();
            installResult.ExitCode.Should().Be(0);
            Thread.Sleep(250);
        }
    }
}
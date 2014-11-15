// <copyright file="NuGetBauTaskBaseFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;
    using System.IO;

    public static class NuGetBauTaskBaseFacts
    {

        [Fact]
        public static void CanFindYourInnerSelf()
        {
            // arrange
            var task = new NuGetRestore(); // NOTE: use a type in the assembly
            var expectedPath = typeof(NuGetBauTaskBase).Assembly.Location; // NOTE: some test runners may have issues with this line

            // act
            var actualPath = task.GetBauNuGetPluginAssemblyPath();

            // assert
            Assert.Equal(expectedPath, actualPath, StringComparer.Ordinal);
        }

        [Fact]
        public static void CanFindNuGetCommandLineExe()
        {
            // explicit
            if (!System.Diagnostics.Debugger.IsAttached)
                return; // this is the best thing I can think of to match [Explicit]

            // arrange
            // NOTE: this test uses scriptcs as an easy way to bring NuGet.exe into the test directory
            var currentDirectory = Path.GetDirectoryName(typeof(NuGetBauTaskBase).Assembly.Location);
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
            var task = new Derrived();
                
            // act
            var actualPath = task.GetNugetCommandLineAssemblyPath();

            // assert
            actualPath.Should().EndWith("NuGet.exe");
            File.Exists(actualPath).Should().BeTrue();
        }

        private class Derrived : NuGetBauTaskBase
        {

        }

    }
}
// <copyright file="NuGetPushFacts.cs" company="Bau contributors">
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

    public static class NuGetPushFacts
    {
        [Fact]
        public static void CanPushUsingCli()
        {
            // arrange
            NuGetCliLocatorFacts.InstallNuGetCli();

            var nugetExePath = NuGetCliLocator.Default.GetNugetCommandLineAssemblyPath();
            var nugetExePackagePath = nugetExePath.Directory.Parent.EnumerateFiles("*.nupkg").Single();
            var nugetFakeFolder = new DirectoryInfo("./fake NuGet dot org/"); // keep the slash on, makes a better test
            var task = new NuGetTask();
            var request = task
                .Push(nugetExePackagePath.FullName)
                .WithWorkingDirectory("./")
                .WithSource(nugetFakeFolder.FullName)
                .WithApiKey("poop")
                .WithTimeout(123)
                .WithDisableBuffering(false);

            if (nugetFakeFolder.Exists)
            {
                nugetFakeFolder.Delete(true);
                Thread.Sleep(100);
            }

            nugetFakeFolder.Create();
            Thread.Sleep(100);

            // act
            task.Execute();

            // assert
            nugetFakeFolder.EnumerateFiles("NuGet.CommandLine.*.nupkg").Should().HaveCount(1);
        }
    }
}

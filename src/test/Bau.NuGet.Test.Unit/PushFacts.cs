﻿// <copyright file="PushFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.IO;
    using System.Linq;
    using System.Threading;
    using FluentAssertions;
    using Xunit;

    public static class PushFacts
    {
        [Fact]
        public static void Pushes()
        {
            // arrange
            var nugetDirectory = NuGetFileFinder.FindFile().Directory;
            nugetDirectory.Should().NotBeNull();
            nugetDirectory.Parent.Should().NotBeNull();
            var nugetPackageFile = nugetDirectory.Parent.EnumerateFiles("*.nupkg").Single();
            var nugetFakeFolder = new DirectoryInfo("./fake NuGet dot org/"); // keep the slash on, makes a better test
            var task = new NuGetTask();
            task
                .Push(nugetPackageFile.FullName)
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

        [Fact]
        public static void CreateMultiplePushCommands()
        {
            // arrange
            var task = new NuGetTask();
            var fakeDirName = "./fake-dir";
            var apiKey = "poo";

            // act
            task.Push(
                new[] { "file1", "file2" },
                r => r
                    .WithWorkingDirectory(fakeDirName)
                    .WithApiKey(apiKey));

            // assert
            task.Commands.Should().HaveCount(2);
            task.Commands.All(r => r.WorkingDirectory == fakeDirName).Should().BeTrue();
            task.Commands.OfType<Push>().All(r => r.ApiKey == apiKey).Should().BeTrue();
            task.Commands.OfType<Push>().Select(x => x.Package).Should().Contain("file1");
            task.Commands.OfType<Push>().Select(x => x.Package).Should().Contain("file2");
        }
    }
}

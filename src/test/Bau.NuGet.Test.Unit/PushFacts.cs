// <copyright file="PushFacts.cs" company="Bau contributors">
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
        // TODO (adamralph): move to a PushFeature acceptance test
        [Fact]
        public static void Pushes()
        {
            // arrange
            var nugetDirectory = NuGetFileFinder.FindFile().Directory;
            nugetDirectory.Should().NotBeNull("NuGet.exe should be in a folder");
            nugetDirectory.Parent.Should().NotBeNull("NuGet.exe should be in a folder with a parent folder");
            var nugetPackageFile = nugetDirectory.Parent.EnumerateFiles("*.nupkg").FirstOrDefault();
            nugetPackageFile.Should().NotBeNull("a .nupkg file should be in the folder above NuGet.exe");

            var pushFolder = new DirectoryInfo("./fake NuGet dot org/"); // keep the slash on, makes a better test
            var task = new NuGetTask();
            task
                .Push(nugetPackageFile.FullName)
                .In("./")
                .Source(pushFolder.FullName)
                .Key("poop")
                .Timeout(123)
                .DisableBuffering(false);

            if (pushFolder.Exists)
            {
                pushFolder.Delete(true);
                Thread.Sleep(100);
            }

            pushFolder.Create();
            Thread.Sleep(100);

            // act
            task.Execute();

            // assert
            pushFolder.EnumerateFiles(nugetPackageFile.Name).Should().HaveCount(1);
        }

        // TODO (adamralph): move to a NuGetTask component test
        [Fact]
        public static void CreateMultiplePushCommands()
        {
            // arrange
            var packages = new[] { "package1", "package2" };
            var workingDirectory = "workingDirectory";
            var apiKey = "poo";
            var task = new NuGetTask();

            // act
            task.Push(
                packages,
                push => push
                    .Key(apiKey)
                    .In(workingDirectory));

            // assert
            task.Commands.Should().HaveCount(packages.Length);
            task.Commands.OfType<Push>().Should().HaveCount(packages.Length);
            task.Commands.OfType<Push>().Select(push => push.Package).Should().BeEquivalentTo(packages);
            task.Commands.OfType<Push>().Should().OnlyContain(push => push.ApiKey == apiKey);
            task.Commands.Should().OnlyContain(command => command.WorkingDirectory == workingDirectory);
        }
    }
}

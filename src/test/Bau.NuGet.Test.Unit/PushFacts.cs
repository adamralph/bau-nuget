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
            var nugetDirectory = new FileInfo(NuGetFileFinder.FindFile()).Directory;
            nugetDirectory.Should().NotBeNull("NuGet.exe should be in a folder");
            nugetDirectory.Parent.Should().NotBeNull("NuGet.exe should be in a folder with a parent folder");
            var nugetPackageFile = nugetDirectory.Parent.EnumerateFiles("*.nupkg").FirstOrDefault();
            nugetPackageFile.Should().NotBeNull("a .nupkg file should be in the folder above NuGet.exe");

            var pushFolder = new DirectoryInfo("./fake NuGet dot org/"); // keep the slash on, makes a better test
            var task = new Push();
            task
                .Files(nugetPackageFile.FullName)
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

        [Fact]
        public static void HasAFluentApi()
        {
            // arrange
            var packages = new[] { "package1", "package2" };
            var workingDirectory = "workingDirectory";
            var apiKey = "poo";
            var task = new Push();

            // act
            task
                .Files(packages)
                .Key(apiKey)
                .In(workingDirectory);

            // assert
            task.Packages.Should().HaveCount(packages.Length);
            task.Packages.Should().BeEquivalentTo(packages);
            task.ApiKey.Should().Be(apiKey);
            task.WorkingDirectory.Should().Be(workingDirectory);
        }
    }
}

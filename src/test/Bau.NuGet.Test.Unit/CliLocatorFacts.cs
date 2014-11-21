// <copyright file="CliLocatorFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public static class CliLocatorFacts
    {
        [Fact]
        public static void CanFindYourInnerSelf()
        {
            // arrange
            var loader = new CliLocator();

            // act
            var path = loader.GetBauNuGetPluginAssemblyPath();

            // assert
            File.Exists(path).Should().BeTrue();
        }

        [Fact]
        public static void CanFindNuGetCommandLineExe()
        {
            // arrange
            var loader = new CliLocator();

            // act
            var path = loader.GetNugetCommandLineAssemblyPath();

            // assert
            path.Should().EndWith("NuGet.exe");
            File.Exists(path).Should().BeTrue();
        }
    }
}
// <copyright file="NuGetCliLocatorFacts.cs" company="Bau contributors">
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
    using BauCore;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public static class NuGetCliLocatorFacts
    {
        [Fact]
        public static void CanFindYourInnerSelf()
        {
            // arrange
            var expectedPath = typeof(NuGetCliLocator).Assembly.Location; // NOTE: some test runners may have issues with this line
            var loader = new NuGetCliLocator();

            // act
            var path = loader.GetBauNuGetPluginAssemblyPath();

            // assert
            File.Exists(path).Should().BeTrue();
        }

        [Fact]
        public static void CanFindNuGetCommandLineExe()
        {
            // arrange
            var loader = new NuGetCliLocator();

            // act
            var actualPath = loader.GetNugetCommandLineAssemblyPath();

            // assert
            actualPath.FullName.Should().EndWith("NuGet.exe");
            actualPath.Exists.Should().BeTrue();
        }
    }
}
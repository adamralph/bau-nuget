// <copyright file="NuGetExeFinderFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public static class NuGetExeFinderFacts
    {
        [Fact]
        public static void FindsExe()
        {
            // act
            var file = new FileInfo(NuGetExeFinder.FindExe());

            // assert
            file.Name.Should().Be("NuGet.exe");
            file.Exists.Should().BeTrue();
        }
    }
}
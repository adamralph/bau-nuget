// <copyright file="NuGetFileFinderFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using FluentAssertions;
    using Xunit;

    public static class NuGetFileFinderFacts
    {
        [Fact]
        public static void FindsFile()
        {
            // arrange
            var finder = new NuGetFileFinder();

            // act
            var file = finder.FindFile();

            // assert
            file.Name.Should().Be("NuGet.exe");
            file.Exists.Should().BeTrue();
        }
    }
}
// <copyright file="NuGetFileFinderFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public static class NuGetFileFinderFacts
    {
        [Fact]
        public static void FindsFile()
        {
            // act
            var file = new FileInfo(NuGetFileFinder.FindFile());

            // assert
            file.Name.Should().Be("NuGet.exe");
            file.Exists.Should().BeTrue();
        }
    }
}
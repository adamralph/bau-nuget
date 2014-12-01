// <copyright file="CommandExtensionsFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public static class CommandExtensionsFacts
    {
        public static class TheVerbosityMethods
        {
            [Fact]
            public static void SetTheVerbosity()
            {
                // arrange
                var @explicit = new DummyCommand();
                var normal = new DummyCommand();
                var quiet = new DummyCommand();
                var detailed = new DummyCommand();

                // act
                @explicit.Verbosity(NuGetVerbosity.Detailed);
                normal.VerbosityNormal();
                quiet.VerbosityQuiet();
                detailed.VerbosityDetailed();

                // assert
                @explicit.Verbosity.Should().Be(NuGetVerbosity.Detailed);
                normal.Verbosity.Should().Be(NuGetVerbosity.Normal);
                quiet.Verbosity.Should().Be(NuGetVerbosity.Quiet);
                detailed.Verbosity.Should().Be(NuGetVerbosity.Detailed);
            }
        }

        public static class TheConfigFileMethod
        {
            [Fact]
            public static void SetsTheConfigFile()
            {
                // arrange
                var command = new DummyCommand();

                // act
                command.ConfigFile("poo.p");

                // assert
                command.ConfigFile.Should().Be("poo.p");
            }
        }

        private class DummyCommand : CommandTask
        {
            protected override IEnumerable<string> CreateCustomCommandLineOptions()
            {
                yield break;
            }
        }
    }
}

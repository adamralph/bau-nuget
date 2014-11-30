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
                @explicit.WithVerbosity(NuGetVerbosity.Detailed);
                normal.WithVerbosityNormal();
                quiet.WithVerbosityQuiet();
                detailed.WithVerbosityDetailed();

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
                command.WithConfigFile("poo.p");

                // assert
                command.ConfigFile.Should().Be("poo.p");
            }
        }

        private class DummyCommand : Command
        {
            protected override IEnumerable<string> CreateCustomCommandLineArguments()
            {
                yield break;
            }
        }
    }
}

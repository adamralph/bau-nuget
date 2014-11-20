// <copyright file="CommandFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public static class CommandFacts
    {
        [Fact]
        public static void PropertyVerbosityCli()
        {
            // arrange
            var defaultCommand = new DummyCommand();
            var abusedCommand = new DummyCommand();
            var levels = new[] { "normal", "quiet", "detailed" };

            // act
            var defaultCommandLineArgs = defaultCommand.CreateCommandLineArguments();
            var commandLines = Array.ConvertAll(
                levels, level => abusedCommand.WithVerbosity(level).CreateCommandLineArguments());

            // assert
            defaultCommandLineArgs.Any(x => x.StartsWith("-Verbosity")).Should().BeFalse();
            for (int i = 0; i < levels.Length; i++)
            {
                commandLines[i].Should().Contain("-Verbosity " + levels[i]);
            }
        }

        [Fact]
        public static void PropertyVerbosityFluent()
        {
            // arrange
            var command = new DummyCommand();
            var normal = new DummyCommand();
            var quiet = new DummyCommand();
            var detailed = new DummyCommand();

            // act
            command.WithVerbosity("Grumbling");
            normal.WithVerbosityNormal();
            quiet.WithVerbosityQuiet();
            detailed.WithVerbosityDetailed();

            // assert
            command.Verbosity.Should().Be("Grumbling");
            Assert.Equal("Normal", normal.Verbosity, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("Quiet", quiet.Verbosity, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("Detailed", detailed.Verbosity, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public static void PropertyNonInteractiveCli()
        {
            // arrange
            var normal = new DummyCommand();

            // act
            var normalArgs = normal.CreateCommandLineArguments();

            // assert
            normalArgs.Should().Contain("-NonInteractive");
        }

        [Fact]
        public static void PropertyConfigFileCli()
        {
            // arrange
            var normal = new DummyCommand();
            var modified = new DummyCommand();
            modified.ConfigFile = "poo.p";

            // act
            var normalArgs = normal.CreateCommandLineArguments();
            var modifiedArgs = modified.CreateCommandLineArguments();

            // assert
            normalArgs.Any(x => x.StartsWith("-ConfigFile")).Should().BeFalse();
            modifiedArgs.Should().Contain("-ConfigFile poo.p");
        }

        [Fact]
        public static void PropertyConfigFileFluent()
        {
            // arrange
            var normal = new DummyCommand();
            var modified = new DummyCommand();

            // act
            modified.WithConfigFile("poo.p");

            // assert
            normal.ConfigFile.Should().BeNull();
            modified.ConfigFile.Should().Be("poo.p");
        }

        private class DummyCommand : Command
        {
        }
    }
}

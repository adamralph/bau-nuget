// <copyright file="CommandFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public static class CommandFacts
    {
        [Fact]
        public static void DoesNotCreateVerbosityArgumentWhenNotSpecified()
        {
            // arrange
            var command = new DummyCommand();

            // act
            var arguments = command.CreateCommandLineArguments().ToArray();

            // assert
            arguments.Should().NotContain("-Verbosity");
        }

        [Theory]
        [InlineData(Verbosity.Normal)]
        [InlineData(Verbosity.Quiet)]
        [InlineData(Verbosity.Detailed)]
        public static void CreatesVerbosityArgumentWhenSpecified(Verbosity verbosity)
        {
            // arrange
            var command = new DummyCommand { Verbosity = verbosity };

            // act
            var arguments = command.CreateCommandLineArguments().ToArray();

            // assert
            arguments.Should().Contain("-Verbosity " + verbosity.ToString().ToLowerInvariant());
        }

        [Fact]
        public static void PropertyVerbosityFluent()
        {
            // arrange
            var normal = new DummyCommand();
            var quiet = new DummyCommand();
            var detailed = new DummyCommand();

            // act
            normal.WithVerbosityNormal();
            quiet.WithVerbosityQuiet();
            detailed.WithVerbosityDetailed();

            // assert
            normal.Verbosity.Should().Be(Verbosity.Normal);
            quiet.Verbosity.Should().Be(Verbosity.Quiet);
            detailed.Verbosity.Should().Be(Verbosity.Detailed);
        }

        [Fact]
        public static void PropertyNonInteractiveCli()
        {
            // arrange
            var normal = new DummyCommand();

            // act
            var arguments = normal.CreateCommandLineArguments();

            // assert
            arguments.Should().Contain("-NonInteractive");
        }

        [Fact]
        public static void PropertyConfigFileCli()
        {
            // arrange
            var normal = new DummyCommand();
            var modified = new DummyCommand { ConfigFile = "poo.p" };

            // act
            var normalArguments = normal.CreateCommandLineArguments();
            var modifiedArguments = modified.CreateCommandLineArguments();

            // assert
            normalArguments.Should().NotContain("-ConfigFile");
            modifiedArguments.Should().Contain("-ConfigFile poo.p");
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
            protected override IEnumerable<string> CreateCustomCommandLineArguments()
            {
                yield break;
            }
        }
    }
}

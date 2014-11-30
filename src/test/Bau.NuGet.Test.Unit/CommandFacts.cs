// <copyright file="CommandFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public static class CommandFacts
    {
        public static class TheCreateCommandLineArgumentsMethod
        {
            [Fact]
            public static void DoesNotCreatesAVerbosityArgumentWhenTheVerbosityIsNotSpecified()
            {
                // arrange
                var command = new DummyCommand();

                // act
                var arguments = command.CreateCommandLineArguments().ToArray();

                // assert
                arguments.Should().NotContain("-Verbosity");
            }

            [Theory]
            [InlineData(NuGetVerbosity.Normal)]
            [InlineData(NuGetVerbosity.Quiet)]
            [InlineData(NuGetVerbosity.Detailed)]
            public static void CreatesAVerbosityArgumentWhenTheVerbosityIsSpecified(NuGetVerbosity verbosity)
            {
                // arrange
                var command = new DummyCommand { Verbosity = verbosity };

                // act
                var arguments = command.CreateCommandLineArguments().ToArray();

                // assert
                arguments.Should().Contain("-Verbosity " + verbosity.ToString().ToLowerInvariant());
            }

            [Fact]
            public static void CreatesANonInteractiveArgumentByDefault()
            {
                // arrange
                var normal = new DummyCommand();

                // act
                var arguments = normal.CreateCommandLineArguments();

                // assert
                arguments.Should().Contain("-NonInteractive");
            }

            [Fact]
            public static void DoesNotCreateANonInteractiveArgumentWhenNonInteractiveIsFalse()
            {
                // arrange
                var normal = new DummyCommand { NonInteractive = false };

                // act
                var arguments = normal.CreateCommandLineArguments();

                // assert
                arguments.Should().NotContain("-NonInteractive");
            }

            [Fact]
            public static void DoesNotCreateAConfigFilePropertyWhenTheConfigFileIsNotSpecified()
            {
                // arrange
                var command = new DummyCommand();

                // act
                var arguments = command.CreateCommandLineArguments();

                // assert
                arguments.Should().NotContain("-ConfigFile");
            }

            [Theory]
            [InlineData(@"", @"-ConfigFile """"")]
            [InlineData(@"poo.p", @"-ConfigFile poo.p")]
            [InlineData(@"poo .p", @"-ConfigFile ""poo .p""")]
            [InlineData(@"poo .p\", @"-ConfigFile ""poo .p/""")]
            public static void CreatesAConfigFilePropertyWhenAConfigFileIsSpecified(
                string configFile, string expectedArgument)
            {
                // arrange
                var command = new DummyCommand { ConfigFile = configFile };

                // act
                var arguments = command.CreateCommandLineArguments();

                // assert
                arguments.Should().Contain(expectedArgument);
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

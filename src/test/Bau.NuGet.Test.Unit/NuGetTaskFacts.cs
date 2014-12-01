// <copyright file="NuGetTaskFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public static class NuGetTaskFacts
    {
        public static class TheCreateCommandLineArgumentsMethod
        {
            [Fact]
            public static void DoesNotCreatesAVerbosityArgumentWhenTheVerbosityIsNotSpecified()
            {
                // arrange
                var task = new DummyTask();

                // act
                var arguments = task.CreateCommandLineOptions().ToArray();

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
                var task = new DummyTask { Verbosity = verbosity };

                // act
                var arguments = task.CreateCommandLineOptions().ToArray();

                // assert
                arguments.Should().Contain("-Verbosity " + verbosity.ToString().ToLowerInvariant());
            }

            [Fact]
            public static void CreatesANonInteractiveArgumentByDefault()
            {
                // arrange
                var task = new DummyTask();

                // act
                var arguments = task.CreateCommandLineOptions();

                // assert
                arguments.Should().Contain("-NonInteractive");
            }

            [Fact]
            public static void DoesNotCreateANonInteractiveArgumentWhenNonInteractiveIsFalse()
            {
                // arrange
                var task = new DummyTask { NonInteractive = false };

                // act
                var arguments = task.CreateCommandLineOptions();

                // assert
                arguments.Should().NotContain("-NonInteractive");
            }

            [Fact]
            public static void DoesNotCreateAConfigFilePropertyWhenTheConfigFileIsNotSpecified()
            {
                // arrange
                var task = new DummyTask();

                // act
                var arguments = task.CreateCommandLineOptions();

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
                var task = new DummyTask { ConfigFile = configFile };

                // act
                var arguments = task.CreateCommandLineOptions();

                // assert
                arguments.Should().Contain(expectedArgument);
            }
        }

        private class DummyTask : NuGetTask
        {
            protected override string OperationName
            {
                get { return "help"; }
            }

            protected override IEnumerable<string> CreateCustomCommandLineOptions()
            {
                yield break;
            }

            protected override IEnumerable<string> GetTargetFiles()
            {
                yield break;
            }
        }
    }
}

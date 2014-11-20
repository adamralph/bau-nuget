// <copyright file="NuGetRequestBaseFacts.cs" company="Bau contributors">
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
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public static class NuGetRequestBaseFacts
    {
        [Fact]
        public static void PropertyVerbosityCli()
        {
            // arrange
            var defaultRequest = new DummyRequest();
            var abusedRequest = new DummyRequest();
            var levels = new[] { "normal", "quiet", "detailed" };

            // act
            var defaultCommandLineArgs = defaultRequest.CreateCommandLineArguments();
            var commandLines = Array.ConvertAll(levels, level => abusedRequest.WithVerbosity(level).CreateCommandLineArguments());
            
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
            var request = new DummyRequest();
            var normal = new DummyRequest();
            var quiet = new DummyRequest();
            var detailed = new DummyRequest();

            // act
            request.WithVerbosity("Grumbling");
            normal.WithVerbosityNormal();
            quiet.WithVerbosityQuiet();
            detailed.WithVerbosityDetailed();

            // assert
            request.Verbosity.Should().Be("Grumbling");
            Assert.Equal("Normal", normal.Verbosity, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("Quiet", quiet.Verbosity, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("Detailed", detailed.Verbosity, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public static void PropertyNonInteractiveCli()
        {
            // arrange
            var normal = new DummyRequest();

            // act
            var normalArgs = normal.CreateCommandLineArguments();

            // assert
            normalArgs.Should().Contain("-NonInteractive");
        }

        [Fact]
        public static void PropertyConfigFileCli()
        {
            // arrange
            var normal = new DummyRequest();
            var modified = new DummyRequest();
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
            var normal = new DummyRequest();
            var modified = new DummyRequest();

            // act
            modified.WithConfigFile("poo.p");

            // assert
            normal.ConfigFile.Should().BeNull();
            modified.ConfigFile.Should().Be("poo.p");
        }

        private class DummyRequest : NuGetRequestBase
        {
        }
    }
}

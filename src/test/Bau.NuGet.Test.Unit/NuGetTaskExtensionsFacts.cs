// <copyright file="NuGetTaskExtensionsFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public static class NuGetTaskExtensionsFacts
    {
        public static class TheVerbosityMethods
        {
            [Fact]
            public static void SetTheVerbosity()
            {
                // arrange
                var @explicit = new DummyTask();
                var normal = new DummyTask();
                var quiet = new DummyTask();
                var detailed = new DummyTask();

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
                var task = new DummyTask();

                // act
                task.ConfigFile("poo.p");

                // assert
                task.ConfigFile.Should().Be("poo.p");
            }
        }

        private class DummyTask : NuGetTask
        {
            protected override string Command
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

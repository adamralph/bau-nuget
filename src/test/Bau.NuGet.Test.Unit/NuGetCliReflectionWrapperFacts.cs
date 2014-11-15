// <copyright file="NuGetCliReflectionWrapperFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public static class NuGetCliReflectionWrapperFacts
    {
        [Fact]
        public static void CanReflectIntoTargetAssembly()
        {
            // arrange
            NuGetCliLoaderFacts.InstallNuGetCli();
            var loader = new NuGetCliLoader();

            // act
            var wrapper = new NuGetCliReflectionWrapper(loader.Assembly);

            // assert
            Assert.NotNull(wrapper.NuGetCliAssembly);
            Assert.NotNull(wrapper.CommandBaseType);
        }

        [Fact]
        public static void CanApplyBasicCommandParameters()
        {
            // arrange
            NuGetCliLoaderFacts.InstallNuGetCli();
            var loader = new NuGetCliLoader();
            var wrapper = new NuGetCliReflectionWrapper(loader.Assembly);
            var request = new DummyCommandRequest
            {
                ConfigFile = "poopy.poop",
                NonInteractive = false,
                Verbosity = "detailed"
            };

            // act
            var command = Activator.CreateInstance(wrapper.RestoreCommandType);
            request.Apply(command);

            // assert
            wrapper.CommandBaseType.GetProperty("ConfigFile").GetValue(command).Should().Be(request.ConfigFile);
            wrapper.CommandBaseType.GetProperty("NonInteractive").GetValue(command).Should().Be(request.NonInteractive);
            Assert.Equal(request.Verbosity, wrapper.CommandBaseType.GetProperty("Verbosity").GetValue(command).ToString(), StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public static void CanApplyBasicDownloadCommandParameters()
        {
            // arrange
            NuGetCliLoaderFacts.InstallNuGetCli();
            var loader = new NuGetCliLoader();
            var wrapper = new NuGetCliReflectionWrapper(loader.Assembly);
            var request = new DummyDownloadCommandRequest
            {
                ConfigFile = "poopy.poop",
                NonInteractive = false,
                Verbosity = "detailed",
                DisableParallelProcessing = true,
                NoCache = true,
                PackageSaveMode = "do whatever",
            };
            request.Source.AddRange(new[] { "some-place", "some-other-place" });

            // act
            var command = Activator.CreateInstance(wrapper.RestoreCommandType);
            request.Apply(command);

            // assert
            wrapper.DownloadCommandBaseType.GetProperty("ConfigFile").GetValue(command).Should().Be(request.ConfigFile);
            wrapper.DownloadCommandBaseType.GetProperty("NonInteractive").GetValue(command).Should().Be(request.NonInteractive);
            Assert.Equal(request.Verbosity, wrapper.DownloadCommandBaseType.GetProperty("Verbosity").GetValue(command).ToString(), StringComparer.OrdinalIgnoreCase);
            wrapper.DownloadCommandBaseType.GetProperty("DisableParallelProcessing").GetValue(command).Should().Be(request.DisableParallelProcessing);
            wrapper.DownloadCommandBaseType.GetProperty("NoCache").GetValue(command).Should().Be(request.NoCache);
            wrapper.DownloadCommandBaseType.GetProperty("PackageSaveMode").GetValue(command).Should().Be(request.PackageSaveMode);
            Assert.Equal<string>(request.Source, (ICollection<string>)wrapper.DownloadCommandBaseType.GetProperty("Source").GetValue(command));
        }

        private class DummyCommandRequest : NuGetCliCommandRequestBase
        {
        }

        private class DummyDownloadCommandRequest : NuGetCliDownloadCommandRequestBase
        {
        }
    }
}

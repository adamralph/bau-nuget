// <copyright file="NuGetCliDownloadCommandRequestBaseFacts.cs" company="Bau contributors">
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

    public static class NuGetCliDownloadCommandRequestBaseFacts
    {
        [Fact]
        public static void PropertySourceCli()
        {
            // arrange
            var normal = new DummyRequest();
            var multiple = new DummyRequest();
            multiple.Source.Add(@"http://source1/api");
            multiple.Source.Add(@"C:\some folder\");

            // act
            var normalArgs = normal.CreateCommandLineArguments();
            var multipleArgs = multiple.CreateCommandLineArguments();

            // assert
            normalArgs.Any(x => x.StartsWith("-Source")).Should().BeFalse();
            multipleArgs.Should().Contain(@"-Source http://source1/api");
            multipleArgs.Any(x => x.StartsWith("-Source") && x.Contains(@"C:\some folder")).Should().BeTrue();
        }

        [Fact]
        public static void PropertySourceFluent()
        {
            // arrange
            var normal = new DummyRequest();
            var multiple = new DummyRequest();

            // act
            multiple
                .WithSource(@"http://source1/api")
                .WithSource(@"C:\some folder\");

            // assert
            normal.Source.Should().BeEmpty();
            multiple.Source.Should().Equal(new[] { @"http://source1/api", @"C:\some folder\" });
        }

        [Fact]
        public static void PropertyNoCacheCli()
        {
            // arrange
            var normal = new DummyRequest();
            var enabled = new DummyRequest();
            enabled.NoCache = true;
            var disabled = new DummyRequest();
            disabled.NoCache = false;
            
            // act
            var normalArgs = normal.CreateCommandLineArguments();
            var enabledArgs = enabled.CreateCommandLineArguments();
            var disabledArgs = disabled.CreateCommandLineArguments();

            // assert
            normalArgs.Should().NotContain("-NoCache");
            enabledArgs.Should().Contain("-NoCache");
            disabledArgs.Should().NotContain("-NoCache");
        }

        [Fact]
        public static void PropertyNoCacheFluent()
        {
            // arrange
            var normal = new DummyRequest();
            var enabled = new DummyRequest();
            var disabled = new DummyRequest();

            // act
            normal.WithNoCache();
            enabled.WithNoCache(true);
            disabled.WithNoCache(false);

            // assert
            normal.NoCache.Should().BeTrue();
            enabled.NoCache.Should().BeTrue();
            disabled.NoCache.Should().BeFalse();
        }

        [Fact]
        public static void PropertyDisableParallelProcessingCli()
        {
            // arrange
            var normal = new DummyRequest();
            var enabled = new DummyRequest();
            enabled.DisableParallelProcessing = true;
            var disabled = new DummyRequest();
            disabled.DisableParallelProcessing = false;

            // act
            var normalArgs = normal.CreateCommandLineArguments();
            var enabledArgs = enabled.CreateCommandLineArguments();
            var disabledArgs = disabled.CreateCommandLineArguments();

            // assert
            normalArgs.Should().NotContain("-DisableParallelProcessing");
            enabledArgs.Should().Contain("-DisableParallelProcessing");
            disabledArgs.Should().NotContain("-DisableParallelProcessing");
        }

        [Fact]
        public static void PropertyDisableParallelProcessingFluent()
        {
            // arrange
            var normal = new DummyRequest();
            var enabled = new DummyRequest();
            var disabled = new DummyRequest();

            // act
            normal.WithDisableParallelProcessing();
            enabled.WithDisableParallelProcessing(true);
            disabled.WithDisableParallelProcessing(false);

            // assert
            normal.DisableParallelProcessing.Should().BeTrue();
            enabled.DisableParallelProcessing.Should().BeTrue();
            disabled.DisableParallelProcessing.Should().BeFalse();
        }

        private class DummyRequest : NuGetCliDownloadCommandRequestBase
        {
        }
    }
}

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
            Assert.NotNull(wrapper.RestoreCommandType);
        }
    }
}

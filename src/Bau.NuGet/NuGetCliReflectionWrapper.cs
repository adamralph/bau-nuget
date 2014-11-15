// <copyright file="NuGetCliReflectionWrapper.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Reflection;

    public class NuGetCliReflectionWrapper
    {
        public NuGetCliReflectionWrapper(Assembly nugetCliAssembly)
        {
            Guard.AgainstNullArgument("nugetCliAssembly", nugetCliAssembly);
            this.NuGetCliAssembly = nugetCliAssembly;
            this.RestoreCommandType = this.NuGetCliAssembly.GetType("NuGet.Commands.RestoreCommand");
        }

        public Assembly NuGetCliAssembly { get; private set; }

        public Type RestoreCommandType { get; private set; }

        public void Restore(NuGetCliRestoreCommandRequest request)
        {
            Guard.AgainstNullArgument("request", request);
            throw new NotImplementedException();
        }
    }
}

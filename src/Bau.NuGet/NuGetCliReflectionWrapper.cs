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
            this.CommandBaseType = this.NuGetCliAssembly.GetType("NuGet.Commands.Command");
            this.DownloadCommandBaseType = this.NuGetCliAssembly.GetType("NuGet.Commands.DownloadCommandBase");
            this.RestoreCommandType = this.NuGetCliAssembly.GetType("NuGet.Commands.RestoreCommand");
        }

        public Assembly NuGetCliAssembly { get; private set; }

        public Type CommandBaseType { get; private set; }

        public Type DownloadCommandBaseType { get; private set; }

        public Type RestoreCommandType { get; private set; }

        public void Restore(NuGetCliRestoreCommandRequest request)
        {
            Guard.AgainstNullArgument("request", request);
            var createRestoreCommand = Activator.CreateInstance(this.RestoreCommandType);
            request.Apply(createRestoreCommand);

            throw new NotImplementedException();
        }
    }
}

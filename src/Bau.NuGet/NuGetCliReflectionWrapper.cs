// <copyright file="NuGetCliReflectionWrapper.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;
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
            this.PhysicalFileSystemType = this.NuGetCliAssembly.GetType("NuGet.PhysicalFileSystem");
            this.CommonConsoleType = this.NuGetCliAssembly.GetType("NuGet.Common.Console");
            this.IFileSystemType = this.NuGetCliAssembly.GetType("NuGet.IFileSystem");
            this.IConsoleType = this.NuGetCliAssembly.GetType("NuGet.Common.IConsole");
            this.ICommandType = this.NuGetCliAssembly.GetType("NuGet.ICommand");
        }

        public Assembly NuGetCliAssembly { get; private set; }

        public Type CommandBaseType { get; private set; }

        public Type DownloadCommandBaseType { get; private set; }

        public Type RestoreCommandType { get; private set; }

        public Type PhysicalFileSystemType { get; private set; }

        public Type CommonConsoleType { get; private set; }

        public Type IFileSystemType { get; private set; }

        public Type IConsoleType { get; private set; }

        public Type ICommandType { get; private set; }

        public void Restore(NuGetCliRestoreCommandRequest request)
        {
            Guard.AgainstNullArgument("request", request);

            // compose commands
            var assemblyCatalog = new AssemblyCatalog(this.NuGetCliAssembly);
            var compositionContainer = new CompositionContainer(assemblyCatalog);
            string workingDirectory = null;
            if (!string.IsNullOrEmpty(request.SolutionDirectory))
            {
                workingDirectory = request.SolutionDirectory;
            }
            else
            {
                workingDirectory = Directory.GetCurrentDirectory();
            }

            var fileSystem = Activator.CreateInstance(this.PhysicalFileSystemType, new object[] { workingDirectory });
            compositionContainer.ComposeExportedValue(fileSystem, this.IFileSystemType);
            compositionContainer.ComposeExportedValue(Activator.CreateInstance(this.CommonConsoleType), this.IConsoleType);
            var nugetCliRestoreCommand = compositionContainer.GetExportedValues(this.ICommandType)
                .First(x => x.GetType() == this.RestoreCommandType);
            compositionContainer.ComposeParts(nugetCliRestoreCommand);

            // prepare the command
            request.Apply(nugetCliRestoreCommand);

            // execute the command
            //var currentDirectory = Directory.GetCurrentDirectory();
            var executeMethod = this.RestoreCommandType.GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public);
            executeMethod.Invoke(nugetCliRestoreCommand, new object[0]);
            //Directory.SetCurrentDirectory(currentDirectory);
        }
    }
}

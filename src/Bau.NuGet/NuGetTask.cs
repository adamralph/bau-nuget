// <copyright file="NuGetTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using BauCore;

    [Obsolete]
    public class NuGetTask : BauTask
    {
        private readonly List<CommandTask> commands = new List<CommandTask>();

        public IEnumerable<CommandTask> Commands
        {
            get { return this.commands.ToArray(); }
        }

        public TCommand Add<TCommand>(TCommand command) where TCommand : CommandTask
        {
            if (command != null)
            {
                this.commands.Add(command);
            }

            return command;
        }

        public RestoreTask Restore(string solutionOrPackagesConfig, Action<RestoreTask> configure = null)
        {
            var restore = new RestoreTask().File(solutionOrPackagesConfig);
            if (configure != null)
            {
                configure(restore);
            }

            return this.Add(restore);
        }

        public IEnumerable<RestoreTask> Restore(
            IEnumerable<string> solutionsOrPackagesConfigs, Action<RestoreTask> configure = null)
        {
            Guard.AgainstNullArgument("solutionsOrPackagesConfigs", solutionsOrPackagesConfigs);

            return solutionsOrPackagesConfigs
                .Select(solutionOrPackagesConfig => this.Restore(solutionOrPackagesConfig, configure))
                .ToArray();
        }

        public PackTask Pack(string nuspecOrProject, Action<PackTask> configure = null)
        {
            var pack = new PackTask().File(nuspecOrProject);
            if (configure != null)
            {
                configure(pack);
            }

            return this.Add(pack);
        }

        public IEnumerable<PackTask> Pack(IEnumerable<string> nuspecsOrProjects, Action<PackTask> configure = null)
        {
            Guard.AgainstNullArgument("nuspecsOrProjects", nuspecsOrProjects);

            return nuspecsOrProjects.Select(projectOrNuSpec => this.Pack(projectOrNuSpec, configure)).ToArray();
        }

        public PushTask Push(string package, Action<PushTask> configure = null)
        {
            var push = new PushTask().File(package);
            if (configure != null)
            {
                configure(push);
            }

            return this.Add(push);
        }

        public IEnumerable<PushTask> Push(IEnumerable<string> packages, Action<PushTask> configure = null)
        {
            Guard.AgainstNullArgument("packages", packages);

            return packages.Select(package => this.Push(package, configure)).ToArray();
        }

        protected override void OnActionsExecuted()
        {
            string fileName = null;
            foreach (var processStartInfo in this.commands.Select(command => new ProcessStartInfo
            {
                FileName = command.NuGetExePathOverride ?? fileName ?? (fileName = NuGetFileFinder.FindFile().FullName),
                Arguments = string.Join(" ", command.CreateCommandLineOptions()),
                WorkingDirectory = command.WorkingDirectory,
                UseShellExecute = false
            }))
            {
                processStartInfo.Run();
            }
        }
    }
}

// <copyright file="NuGetTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BauCore;

    public class NuGetTask : BauTask
    {
        public NuGetTask()
        {
            this.Commands = new List<Command>();
        }

        public List<Command> Commands { get; private set; }

        public TCommand Register<TCommand>(TCommand command) where TCommand : Command
        {
            Guard.AgainstNullArgument("command", command);

            this.Commands.Add(command);
            return command;
        }

        public Restore Restore(string targetSolutionOrPackagesConfig, Action<Restore> configure = null)
        {
            var restore = new Restore()
                .For(targetSolutionOrPackagesConfig);

            if (configure != null)
            {
                configure(restore);
            }

            return this.Register(restore);
        }

        public IEnumerable<Restore> Restore(IEnumerable<string> fileTargets, Action<Restore> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);

            var commands = fileTargets.Select(fileTarget => this.Restore(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        public Pack Pack(string targetProjectOrNuSpec, Action<Pack> configure = null)
        {
            var pack = new Pack()
                .For(targetProjectOrNuSpec);

            if (configure != null)
            {
                configure(pack);
            }

            return this.Register(pack);
        }

        public IEnumerable<Pack> Pack(IEnumerable<string> fileTargets, Action<Pack> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);

            var commands = fileTargets.Select(fileTarget => this.Pack(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        public Push Push(string targetPackage, Action<Push> configure = null)
        {
            var push = new Push()
                .For(targetPackage);

            if (configure != null)
            {
                configure(push);
            }

            return this.Register(push);
        }

        public IEnumerable<Push> Push(IEnumerable<string> fileTargets, Action<Push> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);

            var commands = fileTargets.Select(fileTarget => this.Push(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        protected override void OnActionsExecuted()
        {
            var processStartInfos = this.Commands
                .Where(command => command != null)
                .Select(command => command.CreateProcessStartInfo());
            foreach (var processStartInfo in processStartInfos)
            {
                processStartInfo.Run();
            }
        }
    }
}

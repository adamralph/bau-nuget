// <copyright file="NuGetTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BauCore;

    public class NuGetTask : BauTask
    {
        public NuGetTask()
        {
            this.Requests = new List<Command>();
        }

        public List<Command> Requests { get; private set; }

        public TRequest Register<TRequest>(TRequest request) where TRequest : Command
        {
            Guard.AgainstNullArgument("request", request);
            this.Requests.Add(request);
            return request;
        }

        public Restore Restore(string targetSolutionOrPackagesConfig, Action<Restore> configure = null)
        {
            var request = new Restore()
                .For(targetSolutionOrPackagesConfig);

            if (configure != null)
            {
                configure(request);
            }

            return this.Register(request);
        }

        public IEnumerable<Restore> Restore(IEnumerable<string> fileTargets, Action<Restore> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);
            var commands = fileTargets.Select(fileTarget => this.Restore(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        public Pack Pack(string targetProjectOrNuSpec, Action<Pack> configure = null)
        {
            var request = new Pack()
                .For(targetProjectOrNuSpec);

            if (configure != null)
            {
                configure(request);
            }

            return this.Register(request);
        }

        public IEnumerable<Pack> Pack(IEnumerable<string> fileTargets, Action<Pack> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);
            var commands = fileTargets.Select(fileTarget => this.Pack(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        public Push Push(string targetPackage, Action<Push> configure = null)
        {
            var request = new Push()
                .For(targetPackage);

            if (configure != null)
            {
                configure(request);
            }

            return this.Register(request);
        }

        public IEnumerable<Push> Push(IEnumerable<string> fileTargets, Action<Push> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);
            var commands = fileTargets.Select(fileTarget => this.Push(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        protected override void OnActionsExecuted()
        {
            var processStartInfos = this.Requests
                .Where(request => request != null)
                .Select(request => request.CreateProcessStartInfo());
            foreach (var processStartInfo in processStartInfos)
            {
                processStartInfo.Run();
            }
        }
    }
}

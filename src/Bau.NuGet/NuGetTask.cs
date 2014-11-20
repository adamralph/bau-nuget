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
            this.Requests = new List<NuGetCliCommandRequestBase>();
        }

        public List<NuGetCliCommandRequestBase> Requests { get; private set; }

        public TRequest Register<TRequest>(TRequest request) where TRequest : NuGetCliCommandRequestBase
        {
            Guard.AgainstNullArgument("request", request);
            this.Requests.Add(request);
            return request;
        }

        public NuGetCliRestoreCommandRequest Restore(string targetSolutionOrPackagesConfig, Action<NuGetCliRestoreCommandRequest> configure = null)
        {
            var request = new NuGetCliRestoreCommandRequest()
                .For(targetSolutionOrPackagesConfig);

            if (configure != null)
            {
                configure(request);
            }

            return this.Register(request);
        }

        public IEnumerable<NuGetCliRestoreCommandRequest> Restore(IEnumerable<string> fileTargets, Action<NuGetCliRestoreCommandRequest> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);
            var commands = fileTargets.Select(fileTarget => this.Restore(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        public NuGetCliPackCommandRequest Pack(string targetProjectOrNuSpec, Action<NuGetCliPackCommandRequest> configure = null)
        {
            var request = new NuGetCliPackCommandRequest()
                .For(targetProjectOrNuSpec);

            if (configure != null)
            {
                configure(request);
            }

            return this.Register(request);
        }

        public IEnumerable<NuGetCliPackCommandRequest> Pack(IEnumerable<string> fileTargets, Action<NuGetCliPackCommandRequest> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);
            var commands = fileTargets.Select(fileTarget => this.Pack(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        public NuGetCliPushCommandRequest Push(string targetPackage, Action<NuGetCliPushCommandRequest> configure = null)
        {
            var request = new NuGetCliPushCommandRequest()
                .For(targetPackage);

            if (configure != null)
            {
                configure(request);
            }

            return this.Register(request);
        }

        public IEnumerable<NuGetCliPushCommandRequest> Push(IEnumerable<string> fileTargets, Action<NuGetCliPushCommandRequest> configure = null)
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

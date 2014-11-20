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
            this.Requests = new List<NuGetRequestBase>();
        }

        public List<NuGetRequestBase> Requests { get; private set; }

        public TRequest Register<TRequest>(TRequest request) where TRequest : NuGetRequestBase
        {
            Guard.AgainstNullArgument("request", request);
            this.Requests.Add(request);
            return request;
        }

        public NuGetRestoreRequest Restore(string targetSolutionOrPackagesConfig, Action<NuGetRestoreRequest> configure = null)
        {
            var request = new NuGetRestoreRequest()
                .For(targetSolutionOrPackagesConfig);

            if (configure != null)
            {
                configure(request);
            }

            return this.Register(request);
        }

        public IEnumerable<NuGetRestoreRequest> Restore(IEnumerable<string> fileTargets, Action<NuGetRestoreRequest> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);
            var commands = fileTargets.Select(fileTarget => this.Restore(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        public NuGetPackRequest Pack(string targetProjectOrNuSpec, Action<NuGetPackRequest> configure = null)
        {
            var request = new NuGetPackRequest()
                .For(targetProjectOrNuSpec);

            if (configure != null)
            {
                configure(request);
            }

            return this.Register(request);
        }

        public IEnumerable<NuGetPackRequest> Pack(IEnumerable<string> fileTargets, Action<NuGetPackRequest> configure = null)
        {
            Guard.AgainstNullArgument("fileTargets", fileTargets);
            var commands = fileTargets.Select(fileTarget => this.Pack(fileTarget, configure));
            return commands.ToList(); // NOTE: required to force the enumerable to be iterated
        }

        public NuGetPushRequest Push(string targetPackage, Action<NuGetPushRequest> configure = null)
        {
            var request = new NuGetPushRequest()
                .For(targetPackage);

            if (configure != null)
            {
                configure(request);
            }

            return this.Register(request);
        }

        public IEnumerable<NuGetPushRequest> Push(IEnumerable<string> fileTargets, Action<NuGetPushRequest> configure = null)
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

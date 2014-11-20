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

        public TRequest RegisterRequest<TRequest>(TRequest request) where TRequest : NuGetCliCommandRequestBase
        {
            Guard.AgainstNullArgument("request", request);

            if (this.Requests.Contains(request))
            {
                throw new InvalidOperationException();
            }

            this.Requests.Add(request);
            return request;
        }

        public void RegisterRequests<TRequest>(IEnumerable<TRequest> requests) where TRequest : NuGetCliCommandRequestBase
        {
            Guard.AgainstNullArgument("requests", requests);
            this.Requests.AddRange(requests);
        }

        public NuGetCliRestoreCommandRequest Restore(string targetSolutionOrPackagesConfig)
        {
            return this.RegisterRequest(
                new NuGetCliRestoreCommandRequest()
                .For(targetSolutionOrPackagesConfig));
        }

        public NuGetCliPackCommandRequest Pack(string targetProjectOrNuSpec)
        {
            return this.RegisterRequest(
                new NuGetCliPackCommandRequest()
                .For(targetProjectOrNuSpec));
        }

        public NuGetCliPushCommandRequest Push(string targetPackage)
        {
            return this.RegisterRequest(
                new NuGetCliPushCommandRequest()
                .For(targetPackage));
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

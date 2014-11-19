// <copyright file="NuGetCliRestoreCommandRequest.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;

    public class NuGetCliRestoreCommandRequest : NuGetCliDownloadCommandRequestBase
    {
        public NuGetCliRestoreCommandRequest()
        {
            this.RequireConsent = false;
        }

        public string TargetSolutionOrPackagesConfig { get; set; }

        public bool RequireConsent { get; set; }

        public string PackagesDirectory { get; set; }

        public string SolutionDirectory { get; set; }

        public virtual NuGetCliRestoreCommandRequest For(string targetSolutionOrPackagesConfig)
        {
            this.TargetSolutionOrPackagesConfig = targetSolutionOrPackagesConfig;
            return this;
        }

        public virtual NuGetCliRestoreCommandRequest WithRequiresConsent(bool enabled = true)
        {
            this.RequireConsent = enabled;
            return this;
        }

        public virtual NuGetCliRestoreCommandRequest WithPackagesDirectory(string packagesDirectory)
        {
            this.PackagesDirectory = packagesDirectory;
            return this;
        }

        public virtual NuGetCliRestoreCommandRequest WithSolutionDirectory(string solutionDirectory)
        {
            this.SolutionDirectory = solutionDirectory;
            return this;
        }

        public override System.Collections.Generic.List<string> CreateCommandLineArguments()
        {
            var arguments = new List<string> { "restore" };

            if (!string.IsNullOrWhiteSpace(this.TargetSolutionOrPackagesConfig))
            {
                arguments.Add(this.QuoteWrapCliValue(this.TargetSolutionOrPackagesConfig));
            }

            if (this.RequireConsent)
            {
                arguments.Add("-RequireConsent");
            }

            if (!string.IsNullOrWhiteSpace(this.PackagesDirectory))
            {
                arguments.Add("-PackagesDirectory " + this.QuoteWrapCliValue(this.PackagesDirectory));
            }

            if (!string.IsNullOrWhiteSpace(this.SolutionDirectory))
            {
                arguments.Add("-SolutionDirectory " + this.QuoteWrapCliValue(this.SolutionDirectory));
            }

            arguments.AddRange(base.CreateCommandLineArguments());

            return arguments;
        }
    }
}

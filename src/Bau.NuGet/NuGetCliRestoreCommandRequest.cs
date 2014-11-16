// <copyright file="NuGetCliRestoreCommandRequest.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;

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

        public override void AppendCommandLineOptions(System.Collections.Generic.List<string> argumentBuilder)
        {
            if (!string.IsNullOrWhiteSpace(this.TargetSolutionOrPackagesConfig))
            {
                argumentBuilder.Add("\"" + this.TargetSolutionOrPackagesConfig + "\"");
            }

            if (this.RequireConsent)
            {
                argumentBuilder.Add("-RequireConsent");
            }

            if (!string.IsNullOrWhiteSpace(this.PackagesDirectory))
            {
                argumentBuilder.Add("-PackagesDirectory \"" + this.PackagesDirectory + "\"");
            }

            if (!string.IsNullOrWhiteSpace(this.SolutionDirectory))
            {
                argumentBuilder.Add("-SolutionDirectory \"" + this.SolutionDirectory + "\"");
            }

            base.AppendCommandLineOptions(argumentBuilder);
        }
    }
}

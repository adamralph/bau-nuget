// <copyright file="Restore.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;

    public class Restore : Command
    {
        private readonly HashSet<string> sources = new HashSet<string>();

        public Restore()
        {
            this.RequireConsent = false;
            this.NoCache = false;
            this.DisableParallelProcessing = false;
        }

        public ICollection<string> Sources
        {
            get { return this.sources; }
        }

        public bool NoCache { get; set; }

        public bool DisableParallelProcessing { get; set; }

        public string TargetSolutionOrPackagesConfig { get; set; }

        public bool RequireConsent { get; set; }

        public string PackagesDirectory { get; set; }

        public string SolutionDirectory { get; set; }

        public virtual Restore For(string targetSolutionOrPackagesConfig)
        {
            this.TargetSolutionOrPackagesConfig = targetSolutionOrPackagesConfig;
            return this;
        }

        public virtual Restore WithSource(string source)
        {
            this.Sources.Add(source);
            return this;
        }

        public virtual Restore WithNoCache(bool enabled = true)
        {
            this.NoCache = enabled;
            return this;
        }

        public virtual Restore WithDisableParallelProcessing(bool enabled = true)
        {
            this.DisableParallelProcessing = enabled;
            return this;
        }

        public virtual Restore WithRequiresConsent(bool enabled = true)
        {
            this.RequireConsent = enabled;
            return this;
        }

        public virtual Restore WithPackagesDirectory(string packagesDirectory)
        {
            this.PackagesDirectory = packagesDirectory;
            return this;
        }

        public virtual Restore WithSolutionDirectory(string solutionDirectory)
        {
            this.SolutionDirectory = solutionDirectory;
            return this;
        }

        protected override IEnumerable<string> CreateCommandLineArguments()
        {
            yield return "restore";

            if (!string.IsNullOrWhiteSpace(this.TargetSolutionOrPackagesConfig))
            {
                yield return this.QuoteWrapCliValue(this.TargetSolutionOrPackagesConfig);
            }

            if (this.RequireConsent)
            {
                yield return "-RequireConsent";
            }

            if (!string.IsNullOrWhiteSpace(this.PackagesDirectory))
            {
                yield return "-PackagesDirectory " + this.QuoteWrapCliValue(this.PackagesDirectory);
            }

            if (!string.IsNullOrWhiteSpace(this.SolutionDirectory))
            {
                yield return "-SolutionDirectory " + this.QuoteWrapCliValue(this.SolutionDirectory);
            }

            if (this.NoCache)
            {
                yield return "-NoCache";
            }

            if (this.DisableParallelProcessing)
            {
                yield return "-DisableParallelProcessing";
            }

            foreach (var source in this.Sources)
            {
                yield return "-Source " + this.QuoteWrapCliValue(source);
            }
        }
    }
}

// <copyright file="Restore.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;

    public class Restore : Command
    {
        private readonly HashSet<string> sources = new HashSet<string>();

        public string SolutionOrPackagesConfig { get; set; }

        public bool RequireConsent { get; set; }

        public string PackagesDirectory { get; set; }

        public string SolutionDirectory { get; set; }

        public ICollection<string> Sources
        {
            get { return this.sources; }
        }

        public bool NoCache { get; set; }

        public bool DisableParallelProcessing { get; set; }

        public string PackageSaveMode { get; set; }

        public virtual Restore For(string solutionOrPackagesConfig)
        {
            this.SolutionOrPackagesConfig = solutionOrPackagesConfig;
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

        public virtual Restore WithPackageSaveMode(string packageSaveMode)
        {
            this.PackageSaveMode = packageSaveMode;
            return this;
        }

        protected override IEnumerable<string> CreateCommandLineArguments()
        {
            yield return "restore";

            if (this.SolutionOrPackagesConfig != null)
            {
                yield return this.QuoteWrapCliValue(this.SolutionOrPackagesConfig);
            }

            if (this.RequireConsent)
            {
                yield return "-RequireConsent";
            }

            if (this.PackagesDirectory != null)
            {
                yield return "-PackagesDirectory " + this.QuoteWrapCliValue(this.PackagesDirectory);
            }

            if (this.SolutionDirectory != null)
            {
                yield return "-SolutionDirectory " + this.QuoteWrapCliValue(this.SolutionDirectory);
            }

            foreach (var source in this.Sources)
            {
                yield return "-Source " + this.QuoteWrapCliValue(source);
            }

            if (this.NoCache)
            {
                yield return "-NoCache";
            }

            if (this.DisableParallelProcessing)
            {
                yield return "-DisableParallelProcessing";
            }

            if (this.PackageSaveMode != null)
            {
                yield return "-PackageSaveMode" + this.QuoteWrapCliValue(this.PackageSaveMode);
            }
        }
    }
}

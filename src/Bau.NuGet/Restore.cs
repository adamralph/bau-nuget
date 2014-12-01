// <copyright file="Restore.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;

    public class Restore : NuGetTask
    {
        private readonly List<string> solutionsOrPackagesConfigs = new List<string>();

        private readonly HashSet<string> sources = new HashSet<string>();

        public ICollection<string> SolutionsOrPackagesConfigs
        {
            get { return this.solutionsOrPackagesConfigs; }
        }

        public bool ConsentRequired { get; set; }

        public string PackagesDirectory { get; set; }

        public string SolutionDirectory { get; set; }

        public ICollection<string> Sources
        {
            get { return this.sources; }
        }

        public bool NoCache { get; set; }

        public bool ParallelProcessingDisabled { get; set; }

        public string PackageSaveMode { get; set; }

        protected override string Command
        {
            get { return "restore"; }
        }

        public virtual Restore Files(params string[] solutionsOrPackagesConfigs)
        {
            this.solutionsOrPackagesConfigs.AddRange(solutionsOrPackagesConfigs);
            return this;
        }

        public virtual Restore Files(IEnumerable<string> solutionsOrPackagesConfigs)
        {
            this.solutionsOrPackagesConfigs.AddRange(solutionsOrPackagesConfigs);
            return this;
        }

        public virtual Restore RequiresConsent(bool enabled = true)
        {
            this.ConsentRequired = enabled;
            return this;
        }

        public virtual Restore PackagesIn(string packagesDirectory)
        {
            this.PackagesDirectory = packagesDirectory;
            return this;
        }

        public virtual Restore SolutionIn(string solutionDirectory)
        {
            this.SolutionDirectory = solutionDirectory;
            return this;
        }

        public virtual Restore UseSource(string source)
        {
            this.sources.Add(source);
            return this;
        }

        public virtual Restore UseSource(params string[] sources)
        {
            this.sources.UnionWith(sources);
            return this;
        }

        public virtual Restore DisableCache(bool enabled = true)
        {
            this.NoCache = enabled;
            return this;
        }

        public virtual Restore DisableParallelProcessing(bool enabled = true)
        {
            this.ParallelProcessingDisabled = enabled;
            return this;
        }

        public virtual Restore SaveMode(string packageSaveMode)
        {
            this.PackageSaveMode = packageSaveMode;
            return this;
        }

        protected override IEnumerable<string> CreateCustomCommandLineOptions()
        {
            if (this.ConsentRequired)
            {
                yield return "-RequireConsent";
            }

            if (this.PackagesDirectory != null)
            {
                yield return "-PackagesDirectory " + NuGetTask.EncodeArgumentValue(this.PackagesDirectory);
            }

            if (this.SolutionDirectory != null)
            {
                yield return "-SolutionDirectory " + NuGetTask.EncodeArgumentValue(this.SolutionDirectory);
            }

            foreach (var source in this.Sources)
            {
                yield return "-Source " + NuGetTask.EncodeArgumentValue(source);
            }

            if (this.NoCache)
            {
                yield return "-NoCache";
            }

            if (this.ParallelProcessingDisabled)
            {
                yield return "-DisableParallelProcessing";
            }

            if (this.PackageSaveMode != null)
            {
                yield return "-PackageSaveMode" + NuGetTask.EncodeArgumentValue(this.PackageSaveMode);
            }
        }

        protected override IEnumerable<string> GetTargetFiles()
        {
            return this.SolutionsOrPackagesConfigs;
        }
    }
}

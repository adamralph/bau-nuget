// <copyright file="Restore.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;

    public class RestoreTask : CommandTask
    {
        private readonly List<string> solutionsOrPackagesConfigs = new List<string>();

        private readonly HashSet<string> sources = new HashSet<string>();

        public ICollection<string> SolutionOrPackagesConfig {
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

        public virtual RestoreTask Files(params string[] solutionsOrPackagesConfigs)
        {
            this.solutionsOrPackagesConfigs.AddRange(solutionsOrPackagesConfigs);
            return this;
        }

        public virtual RestoreTask Files(IEnumerable<string> solutionsOrPackagesConfigs)
        {
            this.solutionsOrPackagesConfigs.AddRange(solutionsOrPackagesConfigs);
            return this;
        }

        public virtual RestoreTask RequiresConsent(bool enabled = true)
        {
            this.ConsentRequired = enabled;
            return this;
        }

        public virtual RestoreTask PackagesIn(string packagesDirectory)
        {
            this.PackagesDirectory = packagesDirectory;
            return this;
        }

        public virtual RestoreTask SolutionIn(string solutionDirectory)
        {
            this.SolutionDirectory = solutionDirectory;
            return this;
        }

        public virtual RestoreTask UseSource(string source) {
            this.sources.Add(source);
            return this;
        }

        public virtual RestoreTask UseSource(params string[] sources)
        {
            this.sources.UnionWith(sources);
            return this;
        }

        public virtual RestoreTask DisableCache(bool enabled = true)
        {
            this.NoCache = enabled;
            return this;
        }

        public virtual RestoreTask DisableParallelProcessing(bool enabled = true)
        {
            this.ParallelProcessingDisabled = enabled;
            return this;
        }

        public virtual RestoreTask SaveMode(string packageSaveMode)
        {
            this.PackageSaveMode = packageSaveMode;
            return this;
        }

        protected override IEnumerable<string> CreateCustomCommandLineOptions()
        {
            yield return "restore";

            if (this.SolutionOrPackagesConfig != null)
            {
                yield return CommandTask.EncodeArgumentValue(this.SolutionOrPackagesConfig);
            }

            if (this.ConsentRequired)
            {
                yield return "-RequireConsent";
            }

            if (this.PackagesDirectory != null)
            {
                yield return "-PackagesDirectory " + CommandTask.EncodeArgumentValue(this.PackagesDirectory);
            }

            if (this.SolutionDirectory != null)
            {
                yield return "-SolutionDirectory " + CommandTask.EncodeArgumentValue(this.SolutionDirectory);
            }

            foreach (var source in this.Sources)
            {
                yield return "-Source " + CommandTask.EncodeArgumentValue(source);
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
                yield return "-PackageSaveMode" + CommandTask.EncodeArgumentValue(this.PackageSaveMode);
            }
        }
    }
}

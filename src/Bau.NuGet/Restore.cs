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

        public virtual Restore File(string solutionOrPackagesConfig)
        {
            this.SolutionOrPackagesConfig = solutionOrPackagesConfig;
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

        public virtual Restore UseSource(string source) {
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

        protected override IEnumerable<string> CreateCustomCommandLineArguments()
        {
            yield return "restore";

            if (this.SolutionOrPackagesConfig != null)
            {
                yield return Command.EncodeArgumentValue(this.SolutionOrPackagesConfig);
            }

            if (this.ConsentRequired)
            {
                yield return "-RequireConsent";
            }

            if (this.PackagesDirectory != null)
            {
                yield return "-PackagesDirectory " + Command.EncodeArgumentValue(this.PackagesDirectory);
            }

            if (this.SolutionDirectory != null)
            {
                yield return "-SolutionDirectory " + Command.EncodeArgumentValue(this.SolutionDirectory);
            }

            foreach (var source in this.Sources)
            {
                yield return "-Source " + Command.EncodeArgumentValue(source);
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
                yield return "-PackageSaveMode" + Command.EncodeArgumentValue(this.PackageSaveMode);
            }
        }
    }
}

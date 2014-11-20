// <copyright file="Restore.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;
    using System.Linq;

    public class Restore : Command
    {
        public Restore()
        {
            this.RequireConsent = false;
            this.Source = new List<string>();
            this.NoCache = false;
            this.DisableParallelProcessing = false;
        }

        /// <summary>
        /// Gets a list of the sources that are to be sent to the NuGet command line tool or NuGet.Core .
        /// </summary>
        /// <remarks>
        /// While this property stores multiple sources it has a singular name to match the names used within NuGet.
        /// </remarks>
        public List<string> Source { get; private set; }

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
            this.Source.Add(source);
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

            if (null != this.Source)
            {
                foreach (var source in this.Source.Where(source => !string.IsNullOrWhiteSpace(source)).Distinct())
                {
                    yield return "-Source " + this.QuoteWrapCliValue(source);
                }
            }
        }
    }
}

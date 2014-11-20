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

        public override List<string> CreateCommandLineArguments()
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

            if (this.NoCache)
            {
                arguments.Add("-NoCache");
            }

            if (this.DisableParallelProcessing)
            {
                arguments.Add("-DisableParallelProcessing");
            }

            if (null != this.Source)
            {
                arguments.AddRange(this.Source
                    .Where(source => !string.IsNullOrWhiteSpace(source))
                    .Distinct()
                    .Select(source => "-Source " + this.QuoteWrapCliValue(source)));
            }

            arguments.AddRange(base.CreateCommandLineArguments());

            return arguments;
        }
    }
}

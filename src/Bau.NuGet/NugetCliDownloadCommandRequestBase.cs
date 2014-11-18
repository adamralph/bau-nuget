// <copyright file="NuGetCliDownloadCommandRequestBase.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class NuGetCliDownloadCommandRequestBase : NuGetCliCommandRequestBase
    {
        protected NuGetCliDownloadCommandRequestBase()
        {
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

        public override void AppendCommandLineOptions(List<string> argumentBuilder)
        {
            if (this.NoCache)
            {
                argumentBuilder.Add("-NoCache");
            }

            if (this.DisableParallelProcessing)
            {
                argumentBuilder.Add("-DisableParallelProcessing");
            }

            if (null != this.Source)
            {
                foreach (var source in this.Source.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct())
                {
                    argumentBuilder.Add("-Source " + this.QuoteWrapCliValue(source));
                }
            }

            base.AppendCommandLineOptions(argumentBuilder);
        }
    }
}

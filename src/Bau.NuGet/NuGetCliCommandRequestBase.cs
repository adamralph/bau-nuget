// <copyright file="NuGetCliCommandRequestBase.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;

    public abstract class NuGetCliCommandRequestBase
    {
        protected NuGetCliCommandRequestBase()
        {
            this.NonInteractive = true;
        }

        public string Verbosity { get; set; }

        public bool NonInteractive { get; set; }

        public string ConfigFile { get; set; }

        public virtual void AppendCommandLineOptions(List<string> arguments)
        {
            if (!string.IsNullOrWhiteSpace(this.Verbosity))
            {
                arguments.Add("-Verbosity " + this.Verbosity);
            }

            if (this.NonInteractive)
            {
                arguments.Add("-NonInteractive");
            }

            if (!string.IsNullOrWhiteSpace(this.ConfigFile))
            {
                arguments.Add("-ConfigFile \"" + this.ConfigFile + "\"");
            }
        }
    }
}

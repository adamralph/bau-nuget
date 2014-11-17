// <copyright file="NuGetCliCommandRequestBase.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public abstract class NuGetCliCommandRequestBase
    {
        private static readonly Regex WhiteSpaceRegex = new Regex(@"\s");

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
                arguments.Add("-ConfigFile " + this.QuoteWrapCliValue(this.ConfigFile));
            }
        }

        protected virtual string QuoteWrapCliValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\"";
            }

            if (WhiteSpaceRegex.IsMatch(value))
            {
                var quotedResult = "\"" + value + "\"";
                if (quotedResult.EndsWith("\\\""))
                {
                    // there are better ways to fix this: http://stackoverflow.com/questions/5510343/escape-command-line-arguments-in-c-sharp/12364234
                    quotedResult = quotedResult.Substring(0, quotedResult.Length - 2) + "/\""; // hack: just flip the slash and hope for the best
                }

                return quotedResult;
            }

            return value;
        }
    }
}

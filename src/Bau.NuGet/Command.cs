// <copyright file="Command.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public abstract class Command
    {
        private static readonly Regex whiteSpaceRegex = new Regex(@"\s");

        protected Command()
        {
            this.NonInteractive = true;
        }

        public string WorkingDirectory { get; set; }

        public string NuGetExePathOverride { get; set; }

        public string Verbosity { get; set; }

        public bool NonInteractive { get; set; }

        public string ConfigFile { get; set; }

        public IEnumerable<string> CreateCommandLineArguments()
        {
            foreach (var argument in this.CreateCustomCommandLineArguments())
            {
                yield return argument;
            }

            if (!string.IsNullOrWhiteSpace(this.Verbosity))
            {
                // NOTE: Verbose is a valid flag but it is deprecated in favor of Verbosity and should not be used
                yield return "-Verbosity " + this.Verbosity;
            }

            if (this.NonInteractive)
            {
                yield return "-NonInteractive";
            }

            if (!string.IsNullOrWhiteSpace(this.ConfigFile))
            {
                yield return "-ConfigFile " + this.QuoteWrapCliValue(this.ConfigFile);
            }
        }

        protected abstract IEnumerable<string> CreateCustomCommandLineArguments();

        protected string QuoteWrapCliValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\""; // supply something
            }

            if (whiteSpaceRegex.IsMatch(value))
            {
                // NOTE: single-quotes does not seem to work even though that is a suggested method over double-quotes
                var quotedResult = "\"" + value + "\"";

                // NOTE: there are better ways to fix this:
                // http://stackoverflow.com/questions/5510343/escape-command-line-arguments-in-c-sharp/12364234
                if (quotedResult.EndsWith("\\\""))
                {
                    // HACK: just flip the slash and hope for the best
                    return quotedResult.Substring(0, quotedResult.Length - 2) + "/\"";
                }

                return quotedResult;
            }

            return value;
        }
    }
}

// <copyright file="Command.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using BauCore;

    public abstract class CommandTask : BauTask
    {
        private static readonly Regex containsWhitespaceRegex = new Regex(@"\s");

        protected CommandTask()
        {
            this.NonInteractive = true;
        }

        public string WorkingDirectory { get; set; }

        public string NuGetExePathOverride { get; set; }

        public NuGetVerbosity? Verbosity { get; set; }

        public bool NonInteractive { get; set; }

        public string ConfigFile { get; set; }

        public IEnumerable<string> CreateCommandLineOptions()
        {
            foreach (var argument in this.CreateCustomCommandLineOptions())
            {
                yield return argument;
            }

            if (this.Verbosity.HasValue)
            {
                // NOTE: Verbose is a valid flag but it is deprecated in favor of Verbosity and should not be used
                yield return "-Verbosity " + this.Verbosity.ToString().ToLowerInvariant();
            }

            if (this.NonInteractive)
            {
                yield return "-NonInteractive";
            }

            if (this.ConfigFile != null)
            {
                yield return "-ConfigFile " + EncodeArgumentValue(this.ConfigFile);
            }
        }

        protected static string EncodeArgumentValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\""; // supply something
            }

            if (containsWhitespaceRegex.IsMatch(value))
            {
                // NOTE: single-quotes does not seem to work even though that is a suggested method over double-quotes
                var quotedResult = "\"" + value + "\"";

                // NOTE: there are better ways to fix this:
                // http://stackoverflow.com/questions/5510343/escape-command-line-arguments-in-c-sharp/12364234
                if (quotedResult.EndsWith("\\\"", StringComparison.Ordinal))
                {
                    // HACK: just flip the slash and hope for the best
                    return quotedResult.Substring(0, quotedResult.Length - 2) + "/\"";
                }

                return quotedResult;
            }

            return value;
        }

        protected virtual string GetNuGetExecutableFileInfo()
        {
            if (this.NuGetExePathOverride != null)
            {
                return this.NuGetExePathOverride;
            }

            var detectedLocation = NuGetFileFinder.FindFile();
            if(detectedLocation != null)
            {
                return detectedLocation.FullName;
            }

            return NuGetFileFinder.defaultNuGetExeName;
        }

        protected abstract IEnumerable<string> CreateCustomCommandLineOptions();

        protected abstract override void OnActionsExecuted();
    }
}

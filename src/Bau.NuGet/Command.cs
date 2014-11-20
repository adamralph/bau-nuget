// <copyright file="Command.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;

    public abstract class Command
    {
        private static readonly Regex WhiteSpaceRegex = new Regex(@"\s");

        protected Command()
        {
            this.WorkingDirectory = null;
            this.NuGetExePathOverride = null;
            this.Verbosity = null;
            this.NonInteractive = true;
            this.ConfigFile = null;
        }

        public string WorkingDirectory { get; set; }

        public string NuGetExePathOverride { get; set; }

        public string Verbosity { get; set; }

        public bool NonInteractive { get; private set; }

        public string ConfigFile { get; set; }

        public virtual List<string> CreateCommandLineArguments()
        {
            var arguments = new List<string>();

            if (!string.IsNullOrWhiteSpace(this.Verbosity))
            {
                // NOTE: Verbose is a valid flag but it is deprecated in favor of Verbosity and should not be used
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

            return arguments;
        }

        public virtual ProcessStartInfo CreateProcessStartInfo()
        {
            string nugetExePath;

            if (!string.IsNullOrWhiteSpace(this.NuGetExePathOverride))
            {
                nugetExePath = this.NuGetExePathOverride; // TODO: should this be converted to a full path?
            }
            else
            {
                var fileInfo = CliLocator.Default.GetNugetCommandLineAssemblyPath();
                if (fileInfo != null)
                {
                    nugetExePath = fileInfo.FullName;
                }
                else
                {
                    throw new FileNotFoundException("NuGet.exe");
                }
            }

            return new ProcessStartInfo
            {
                FileName = nugetExePath,
                Arguments = string.Join(" ", this.CreateCommandLineArguments()),
                WorkingDirectory = !string.IsNullOrWhiteSpace(this.WorkingDirectory)
                    ? Path.GetFullPath(this.WorkingDirectory)
                    : Directory.GetCurrentDirectory(),
                UseShellExecute = false
            };
        }

        protected virtual string QuoteWrapCliValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "\"\""; // supply something
            }

            if (WhiteSpaceRegex.IsMatch(value))
            {
                // NOTE: single-quotes does not seem to work even though that is a suggested method over double-quotes
                var quotedResult = "\"" + value + "\"";

                // NOTE: there are better ways to fix this: http://stackoverflow.com/questions/5510343/escape-command-line-arguments-in-c-sharp/12364234
                if (quotedResult.EndsWith("\\\""))
                {
                    quotedResult = quotedResult.Substring(0, quotedResult.Length - 2) + "/\""; // HACK: just flip the slash and hope for the best
                }

                return quotedResult;
            }

            return value;
        }
    }
}

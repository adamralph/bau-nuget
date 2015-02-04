// <copyright file="NuGetTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using BauCore;

    public abstract class NuGetTask : BauTask
    {
        private static readonly Regex containsWhitespaceRegex = new Regex(@"\s");

        private readonly List<string> extraArgs = new List<string>();

        protected NuGetTask()
        {
            this.NonInteractive = true;
        }

        public IEnumerable<string> Files { get; set; }

        public string WorkingDirectory { get; set; }

        public string Exe { get; set; }

        public NuGetVerbosity? Verbosity { get; set; }

        public bool NonInteractive { get; set; }

        public string ConfigFile { get; set; }

        public ICollection<string> ExtraArgs
        {
            get { return this.extraArgs; }
        }

        protected abstract string Command { get; }

        public IEnumerable<string> CreateCommandLineOptions()
        {
            foreach (var option in this.CreateCustomCommandLineOptions())
            {
                yield return option;
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

            foreach (var arg in this.ExtraArgs)
            {
                // NOTE: do not encode the extra arguments as they may need to be used as is
                yield return arg;
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

        protected override void OnActionsExecuted()
        {
            var fileName = this.Exe ?? NuGetExeFinder.FindExe();
            
            string[] fileArray;
            if (this.Files  == null || (fileArray = this.Files.ToArray()).Length == 0)
            {
                this.LogInfo("No files specified.");
                return;
            }

            var options = this.CreateCommandLineOptions();
            var processStartInfos = fileArray
                .Select(EncodeArgumentValue)
                .Select(file => new[] { this.Command, file }.Concat(options))
                .Select(argument => string.Join(" ", argument))
                .Select(arguments => new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    WorkingDirectory = this.WorkingDirectory,
                    UseShellExecute = false
                });

            foreach (var processStartInfo in processStartInfos)
            {
                processStartInfo.Run();
            }
        }

        protected abstract IEnumerable<string> CreateCustomCommandLineOptions();
    }
}

// <copyright file="NuGetCliPackCommandRequest.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NuGetCliPackCommandRequest : NuGetCliCommandRequestBase
    {
        public NuGetCliPackCommandRequest()
        {
            this.Exclude = new List<string>();
            this.Properties = new Dictionary<string, string>();
        }

        public string TargetProjectOrNuSpect { get; set; }

        public string OutputDirectory { get; set; }

        public string BasePath { get; set; }

        public string Version { get; set; }

        public List<string> Exclude { get; set; }

        public bool Symbols { get; set; }

        public bool Tool { get; set; }

        public bool Build { get; set; }

        public bool NoDefaultExcludes { get; set; }

        public bool NoPackageAnalysis { get; set; }

        public bool ExcludeEmptyDirectories { get; set; }

        public bool IncludeReferencedProjects { get; set; }

        public Dictionary<string, string> Properties { get; set; }

        public string MinClientVersion { get; set; }

        public override void AppendCommandLineOptions(System.Collections.Generic.List<string> arguments)
        {
            // NOTE: Verbose is a valid flag but it is deprecated in favor of Verbosity
            if (!string.IsNullOrWhiteSpace(this.TargetProjectOrNuSpect))
            {
                arguments.Add("\"" + this.TargetProjectOrNuSpect + "\"");
            }

            if (!string.IsNullOrWhiteSpace(this.OutputDirectory))
            {
                arguments.Add("-OutputDirectory \"" + this.OutputDirectory + "\"");
            }

            if (!string.IsNullOrWhiteSpace(this.BasePath))
            {
                arguments.Add("-BasePath \"" + this.BasePath + "\"");
            }

            if (!string.IsNullOrWhiteSpace(this.Version))
            {
                arguments.Add("-Version \"" + this.Version + "\"");
            }

            if (this.Exclude != null)
            {
                foreach (var exclude in this.Exclude.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    arguments.Add("-Exclude \"" + exclude + "\"");
                }
            }

            if (this.Symbols)
            {
                arguments.Add("-Symbols");
            }

            if (this.Tool)
            {
                arguments.Add("-Tool");
            }

            if (this.Build)
            {
                arguments.Add("-Build");
            }

            if (this.NoDefaultExcludes)
            {
                arguments.Add("-NoDefaultExcludes");
            }

            if (this.NoPackageAnalysis)
            {
                arguments.Add("-NoPackageAnalysis");
            }

            if (this.ExcludeEmptyDirectories)
            {
                arguments.Add("-ExcludeEmptyDirectories");
            }

            if (this.IncludeReferencedProjects)
            {
                arguments.Add("-IncludeReferencedProjects");
            }

            if (this.Properties != null)
            {
                var propertyParts = this.Properties
                    .Where(set => !string.IsNullOrWhiteSpace(set.Key))
                    .Select(set => string.Concat(set.Key + "=" + set.Value))
                    .ToList();
                if (propertyParts.Count > 0)
                {
                    arguments.Add("-Properties " + string.Join(";", propertyParts));
                }
            }

            if (!string.IsNullOrWhiteSpace(this.MinClientVersion))
            {
                arguments.Add("-MinClientVersion \"" + this.MinClientVersion + "\"");
            }

            base.AppendCommandLineOptions(arguments);
        }
    }
}

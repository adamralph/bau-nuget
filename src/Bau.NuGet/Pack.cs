// <copyright file="Pack.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Pack : Command
    {
        public Pack()
        {
            this.Exclude = new List<string>();
            this.Properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string TargetProjectOrNuSpec { get; set; }

        public string OutputDirectory { get; set; }

        public string BasePath { get; set; }

        public string Version { get; set; }

        public List<string> Exclude { get; private set; }

        public bool Symbols { get; set; }

        public bool Tool { get; set; }

        public bool Build { get; set; }

        public bool NoDefaultExcludes { get; set; }

        public bool NoPackageAnalysis { get; set; }

        public bool ExcludeEmptyDirectories { get; set; }

        public bool IncludeReferencedProjects { get; set; }

        public Dictionary<string, string> Properties { get; private set; }

        public string MiniClientVersion { get; set; }

        public void AddExcludes(params string[] excludes)
        {
            if (this.Exclude == null)
            {
                this.Exclude = new List<string>();
            }

            this.Exclude.AddRange(excludes);
        }

        public void SetProperty(string key, string value)
        {
            if (this.Properties == null)
            {
                this.Properties = new Dictionary<string, string>();
            }

            this.Properties[key] = value;
        }

        public Pack For(string targetProjectOrNuSpec)
        {
            this.TargetProjectOrNuSpec = targetProjectOrNuSpec;
            return this;
        }

        public Pack WithOutputDirectory(string outputDirectory)
        {
            this.OutputDirectory = outputDirectory;
            return this;
        }

        public Pack WithBasePath(string basePath)
        {
            this.BasePath = basePath;
            return this;
        }

        public Pack WithVersion(string version)
        {
            this.Version = version;
            return this;
        }

        public Pack WithExclude(params string[] excludes)
        {
            this.AddExcludes(excludes);
            return this;
        }

        public Pack WithProperty(string key, string value)
        {
            this.SetProperty(key, value);
            return this;
        }

        public Pack WithMiniClientVersion(string version)
        {
            this.MiniClientVersion = version;
            return this;
        }

        public Pack WithSymbols(bool enabled = true)
        {
            this.Symbols = enabled;
            return this;
        }

        public Pack WithTool(bool enabled = true)
        {
            this.Tool = enabled;
            return this;
        }

        public Pack WithBuild(bool enabled = true)
        {
            this.Build = enabled;
            return this;
        }

        public Pack WithNoDefaultExcludes(bool enabled = true)
        {
            this.NoDefaultExcludes = enabled;
            return this;
        }

        public Pack WithNoPackageAnalysis(bool enabled = true)
        {
            this.NoPackageAnalysis = enabled;
            return this;
        }

        public Pack WithExcludeEmptyDirectories(bool enabled = true)
        {
            this.ExcludeEmptyDirectories = enabled;
            return this;
        }

        public Pack WithIncludeReferencedProjects(bool enabled = true)
        {
            this.IncludeReferencedProjects = enabled;
            return this;
        }

        public string ExtractVersionString()
        {
            if (!string.IsNullOrWhiteSpace(this.Version))
            {
                return this.Version;
            }

            string result = null;
            if (this.Properties != null)
            {
                this.Properties.TryGetValue("VERSION", out result);
            }

            return result;
        }

        protected override IEnumerable<string> CreateCommandLineArguments()
        {
            yield return "pack";

            if (!string.IsNullOrWhiteSpace(this.TargetProjectOrNuSpec))
            {
                yield return this.QuoteWrapCliValue(this.TargetProjectOrNuSpec);
            }

            if (!string.IsNullOrWhiteSpace(this.OutputDirectory))
            {
                yield return "-OutputDirectory " + this.QuoteWrapCliValue(this.OutputDirectory);
            }

            if (!string.IsNullOrWhiteSpace(this.BasePath))
            {
                yield return "-BasePath " + this.QuoteWrapCliValue(this.BasePath);
            }

            if (!string.IsNullOrWhiteSpace(this.Version))
            {
                yield return "-Version " + this.QuoteWrapCliValue(this.Version);
            }

            if (this.Symbols)
            {
                yield return "-Symbols";
            }

            if (this.Tool)
            {
                yield return "-Tool";
            }

            if (this.Build)
            {
                yield return "-Build";
            }

            if (this.NoDefaultExcludes)
            {
                yield return "-NoDefaultExcludes";
            }

            if (this.NoPackageAnalysis)
            {
                yield return "-NoPackageAnalysis";
            }

            if (this.ExcludeEmptyDirectories)
            {
                yield return "-ExcludeEmptyDirectories";
            }

            if (this.IncludeReferencedProjects)
            {
                yield return "-IncludeReferencedProjects";
            }

            if (!string.IsNullOrWhiteSpace(this.MiniClientVersion))
            {
                yield return "-MinClientVersion " + this.QuoteWrapCliValue(this.MiniClientVersion);
            }

            if (this.Exclude != null)
            {
                foreach (var exclude in this.Exclude.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct())
                {
                    yield return "-Exclude " + this.QuoteWrapCliValue(exclude);
                }
            }

            // properties should be added last to prevent issues with spaces and other characters
            if (this.Properties != null)
            {
                var propertyParts = this.Properties
                    .Where(set => !string.IsNullOrWhiteSpace(set.Key))
                    .Select(set => string.Concat(set.Key, "=", set.Value))
                    .ToList();
                var propertyPartsJoined = string.Join(";", propertyParts);
                if (propertyParts.Count > 0)
                {
                    yield return "-Properties " + this.QuoteWrapCliValue(propertyPartsJoined);
                }
            }
        }
    }
}

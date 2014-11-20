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

        public override List<string> CreateCommandLineArguments()
        {
            var arguments = new List<string> { "pack" };

            if (!string.IsNullOrWhiteSpace(this.TargetProjectOrNuSpec))
            {
                arguments.Add(this.QuoteWrapCliValue(this.TargetProjectOrNuSpec));
            }

            if (!string.IsNullOrWhiteSpace(this.OutputDirectory))
            {
                arguments.Add("-OutputDirectory " + this.QuoteWrapCliValue(this.OutputDirectory));
            }

            if (!string.IsNullOrWhiteSpace(this.BasePath))
            {
                arguments.Add("-BasePath " + this.QuoteWrapCliValue(this.BasePath));
            }

            if (!string.IsNullOrWhiteSpace(this.Version))
            {
                arguments.Add("-Version " + this.QuoteWrapCliValue(this.Version));
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

            if (!string.IsNullOrWhiteSpace(this.MiniClientVersion))
            {
                arguments.Add("-MinClientVersion " + this.QuoteWrapCliValue(this.MiniClientVersion));
            }

            if (this.Exclude != null)
            {
                foreach (var exclude in this.Exclude.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct())
                {
                    arguments.Add("-Exclude " + this.QuoteWrapCliValue(exclude));
                }
            }

            arguments.AddRange(base.CreateCommandLineArguments());

            // properties should be added last to prevent issues with spaces and other characters
            if (this.Properties != null)
            {
                var propertyParts = this.Properties
                    .Where(set => !string.IsNullOrWhiteSpace(set.Key))
                    .Select(set => string.Concat(set.Key + "=" + set.Value))
                    .ToList();
                var propertyPartsJoined = string.Join(";", propertyParts);
                if (propertyParts.Count > 0)
                {
                    arguments.Add("-Properties " + this.QuoteWrapCliValue(propertyPartsJoined));
                }
            }

            return arguments;
        }
    }
}

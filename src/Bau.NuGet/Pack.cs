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
        private readonly HashSet<string> exclusions = new HashSet<string>();
        private readonly Dictionary<string, string> properties =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string NuSpecOrProject { get; set; }

        public string OutputDirectory { get; set; }

        public string BasePath { get; set; }

        public string Version { get; set; }

        public ICollection<string> Exclusions
        {
            get { return this.exclusions; }
        }

        public bool Symbols { get; set; }

        public bool Tool { get; set; }

        public bool Build { get; set; }

        public bool NoDefaultExcludes { get; set; }

        public bool NoPackageAnalysis { get; set; }

        public bool ExcludeEmptyDirectories { get; set; }

        public bool IncludeReferencedProjects { get; set; }

        public IDictionary<string, string> Properties
        {
            get { return this.properties; }
        }

        public string MiniClientVersion { get; set; }

        public void AddExcludes(params string[] excludes)
        {
            this.exclusions.UnionWith(excludes);
        }

        public void SetProperty(string key, string value)
        {
            this.properties[key] = value;
        }

        public Pack For(string nuspecOrProject)
        {
            this.NuSpecOrProject = nuspecOrProject;
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

        protected override IEnumerable<string> CreateCustomCommandLineArguments()
        {
            yield return "pack";

            if (this.NuSpecOrProject != null)
            {
                yield return Command.EncodeArgumentValue(this.NuSpecOrProject);
            }

            if (this.OutputDirectory != null)
            {
                yield return "-OutputDirectory " + Command.EncodeArgumentValue(this.OutputDirectory);
            }

            if (this.BasePath != null)
            {
                yield return "-BasePath " + Command.EncodeArgumentValue(this.BasePath);
            }

            if (this.Version != null)
            {
                yield return "-Version " + Command.EncodeArgumentValue(this.Version);
            }

            foreach (var exclusion in this.Exclusions)
            {
                yield return "-Exclude " + Command.EncodeArgumentValue(exclusion);
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

            if (this.properties.Any())
            {
                var value = string.Join(
                    ";", this.Properties.Select(property => string.Concat(property.Key, "=", property.Value)));

                yield return "-Properties " + Command.EncodeArgumentValue(value);
            }

            if (this.MiniClientVersion != null)
            {
                yield return "-MinClientVersion " + Command.EncodeArgumentValue(this.MiniClientVersion);
            }
        }
    }
}

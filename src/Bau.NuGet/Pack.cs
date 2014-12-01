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

        public string NuSpecBasePath { get; set; }

        public string VersionValue { get; set; }

        public ICollection<string> Exclusions
        {
            get { return this.exclusions; }
        }

        public bool Symbols { get; set; }

        public bool Tool { get; set; }

        public bool Build { get; set; }

        public bool NoDefaultExcludes { get; set; }

        public bool NoPackageAnalysis { get; set; }

        public bool EmptyDirectoriesExcluded { get; set; }

        public bool ReferencedProjectsIncluded { get; set; }

        public IDictionary<string, string> PropertiesCollection
        {
            get { return this.properties; }
        }

        public string MiniClientVersionValue { get; set; }

        public void AddExcludes(params string[] excludes)
        {
            this.exclusions.UnionWith(excludes);
        }

        public Pack File(string nuspecOrProject)
        {
            this.NuSpecOrProject = nuspecOrProject;
            return this;
        }

        public Pack Output(string outputDirectory)
        {
            this.OutputDirectory = outputDirectory;
            return this;
        }

        public Pack NuSpecBase(string basePath)
        {
            this.NuSpecBasePath = basePath;
            return this;
        }

        public Pack Version(string version)
        {
            this.VersionValue = version;
            return this;
        }

        public Pack Exclude(params string[] excludes)
        {
            this.AddExcludes(excludes);
            return this;
        }

        public Pack MakeSymbols(bool enabled = true)
        {
            this.Symbols = enabled;
            return this;
        }

        public Pack AsTool(bool enabled = true)
        {
            this.Tool = enabled;
            return this;
        }

        public Pack PerformBuild(bool enabled = true)
        {
            this.Build = enabled;
            return this;
        }

        public Pack DisableDefaultExcludes(bool enabled = true)
        {
            this.NoDefaultExcludes = enabled;
            return this;
        }

        public Pack DisablePackageAnalysis(bool enabled = true)
        {
            this.NoPackageAnalysis = enabled;
            return this;
        }

        public Pack ExcludeEmptyDirectories(bool enabled = true)
        {
            this.EmptyDirectoriesExcluded = enabled;
            return this;
        }

        public Pack IncludeReferencedProjects(bool enabled = true)
        {
            this.ReferencedProjectsIncluded = enabled;
            return this;
        }

        public Pack Property(string key, string value)
        {
            this.properties[key] = value;
            return this;
        }

        public Pack Properties(IDictionary<string, string> pairs)
        {
            foreach (var pair in pairs)
            {
                this.properties[pair.Key] = pair.Value;
            }
            return this;
        }

        public Pack MiniClientVersion(string version)
        {
            this.MiniClientVersionValue = version;
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

            if (this.NuSpecBasePath != null)
            {
                yield return "-BasePath " + Command.EncodeArgumentValue(this.NuSpecBasePath);
            }

            if (this.VersionValue != null)
            {
                yield return "-Version " + Command.EncodeArgumentValue(this.VersionValue);
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

            if (this.EmptyDirectoriesExcluded)
            {
                yield return "-ExcludeEmptyDirectories";
            }

            if (this.ReferencedProjectsIncluded)
            {
                yield return "-IncludeReferencedProjects";
            }

            if (this.properties.Any())
            {
                var value = string.Join(
                    ";", this.PropertiesCollection.Select(property => string.Concat(property.Key, "=", property.Value)));

                yield return "-Properties " + Command.EncodeArgumentValue(value);
            }

            if (this.MiniClientVersionValue != null)
            {
                yield return "-MinClientVersion " + Command.EncodeArgumentValue(this.MiniClientVersionValue);
            }
        }
    }
}

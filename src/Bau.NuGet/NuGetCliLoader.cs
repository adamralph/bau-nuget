﻿// <copyright file="NuGetCliLoader.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class NuGetCliLoader
    {
        public NuGetCliLoader()
        {
            this.LasyNugetCliFileInfo = new Lazy<FileInfo>(this.GetNugetCommandLineAssemblyPath, true);
            this.LasyNugetCliAssembly = new Lazy<Assembly>(this.LoadNugetCliAssembly, true);
        }

        public Assembly Assembly
        {
            get
            {
                return this.LasyNugetCliAssembly.Value;
            }
        }

        private Lazy<FileInfo> LasyNugetCliFileInfo { get; set; }

        private Lazy<Assembly> LasyNugetCliAssembly { get; set; }

        public string GetBauNuGetPluginAssemblyPath()
        {
            var assembly = typeof(NuGetBauTaskBase).Assembly;
            Uri codeBaseUri;
            if (Uri.TryCreate(assembly.CodeBase, UriKind.Absolute, out codeBaseUri))
            {
                var localPath = codeBaseUri.LocalPath;
                if (!string.IsNullOrEmpty(localPath))
                {
                    return localPath;
                }
            }

            return assembly.Location;
        }

        public FileInfo GetNugetCommandLineAssemblyPath()
        {
            const string PackagesSearchDirectoryName = "*packages";
            var currentAssemblyLocation = this.GetBauNuGetPluginAssemblyPath();
            var currentSearchDirectory = new DirectoryInfo(Path.GetDirectoryName(currentAssemblyLocation));
            do
            {
                var localNugetCliFile = currentSearchDirectory
                    .EnumerateDirectories(PackagesSearchDirectoryName)
                    .Select(this.SearchPackageDirectoryForNuGet)
                    .FirstOrDefault(x => x != null);
                if (localNugetCliFile != null)
                {
                    return localNugetCliFile;
                }

                currentSearchDirectory = currentSearchDirectory.Parent;
            }
            while (currentSearchDirectory != null);
            return null;
        }

        private Assembly LoadNugetCliAssembly()
        {
            var cliAssemblyFileInfo = this.LasyNugetCliFileInfo.Value;
            if (cliAssemblyFileInfo == null)
            {
                return null;
            }

            return Assembly.LoadFrom(cliAssemblyFileInfo.FullName);
        }

        private FileInfo SearchPackageDirectoryForNuGet(DirectoryInfo packageDirectory)
        {
            const string NugetCliFolderNameSearch = "NuGet.CommandLine.*";
            const string LocalNuGetPath = "tools/NuGet.exe";
            foreach (var result in packageDirectory.EnumerateDirectories(NugetCliFolderNameSearch, SearchOption.TopDirectoryOnly))
            {
                var expectedPath = Path.Combine(result.FullName, LocalNuGetPath);
                var fileInfo = new FileInfo(expectedPath);
                if (fileInfo.Exists)
                {
                    return fileInfo;
                }
            }

            return null;
        }
    }
}

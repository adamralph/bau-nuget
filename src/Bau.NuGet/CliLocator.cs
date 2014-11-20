// <copyright file="CliLocator.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CliLocator
    {
        static CliLocator()
        {
            Default = new CliLocator();
        }

        public CliLocator()
        {
            this.LasyNugetCliFileInfo = new Lazy<FileInfo>(this.GetNugetCommandLineAssemblyPath, true);
        }

        public static CliLocator Default { get; private set; }

        private Lazy<FileInfo> LasyNugetCliFileInfo { get; set; }

        public FileInfo GetNugetCommandLineAssemblyPath()
        {
            var searchStartDirectories = new List<DirectoryInfo>();
            
            var assemblyLocation = new FileInfo(this.GetBauNuGetPluginAssemblyPath());
            searchStartDirectories.Add(assemblyLocation.Directory);

            var currentWorkingDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            if (currentWorkingDirectory.FullName != assemblyLocation.Directory.FullName)
            {
                searchStartDirectories.Add(currentWorkingDirectory);
            }

            return searchStartDirectories
                .Select(this.GetNugetCommandLineAssemblyPath)
                .FirstOrDefault(f => f != null);
        }

        public FileInfo GetNugetCommandLineAssemblyPath(DirectoryInfo startSearchFromDirectory)
        {
            const string PackagesSearchDirectoryName = "*packages";
            var currentSearchDirectory = startSearchFromDirectory;
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

        public string GetBauNuGetPluginAssemblyPath()
        {
            var assembly = typeof(CliLocator).Assembly;
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

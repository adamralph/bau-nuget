// <copyright file="NuGetBauTaskBase.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BauCore;
    using ScriptCs.Contracts;
    using System.IO;

    public abstract class NuGetBauTaskBase : BauTask
    {

        public string GetBauNuGetPluginAssemblyPath()
        {
            var assembly = typeof(NuGetBauTaskBase).Assembly;
            Uri codeBaseUri;
            if (Uri.TryCreate(assembly.CodeBase, UriKind.Absolute, out codeBaseUri))
            {
                var localPath = codeBaseUri.LocalPath;
                if (!String.IsNullOrEmpty(localPath))
                    return localPath;
            }

            return assembly.Location;
        }

        public FileInfo GetNugetCommandLineAssemblyPath()
        {
            const string packagesSearchDirectoryName = "*packages";
            var currentAssemblyLocation = GetBauNuGetPluginAssemblyPath();
            var currentSearchDirectory = new DirectoryInfo(Path.GetDirectoryName(currentAssemblyLocation));
            do
            {
                var localNugetPath = currentSearchDirectory
                    .EnumerateDirectories(packagesSearchDirectoryName)
                    .Select(SearchPackageDirectoryForNuGet)
                    .FirstOrDefault(x => x != null);
                if (localNugetPath != null)
                    return localNugetPath;

                currentSearchDirectory = currentSearchDirectory.Parent;
            } while (currentSearchDirectory != null);
            return null;
        }

        private FileInfo SearchPackageDirectoryForNuGet(DirectoryInfo packageDirectory)
        {
            const string nugetCliFolderNameSearch = "NuGet.CommandLine.*";
            const string localNuGetPath = "tools/NuGet.exe";
            foreach (var result in packageDirectory.EnumerateDirectories(nugetCliFolderNameSearch, SearchOption.TopDirectoryOnly))
            {
                var expectedPath = Path.Combine(result.FullName, localNuGetPath);
                var fileInfo = new FileInfo(expectedPath);
                if (fileInfo.Exists)
                    return fileInfo;
            }
            return null;
        }

    }
}

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

        public string GetNugetCommandLineAssemblyPath()
        {
            const string packagesDirectoryName = "packages";
            var currentAssemblyLocation = GetBauNuGetPluginAssemblyPath();
            var currentSearchDirectory = new DirectoryInfo(Path.GetDirectoryName(currentAssemblyLocation));
            while (currentSearchDirectory != null)
            {
                if (currentSearchDirectory.Name == packagesDirectoryName)
                {
                    var selfNugetPath = SearchPackageDirectoryForNuGet(currentSearchDirectory);
                    if (selfNugetPath != null)
                        return selfNugetPath;
                }

                var localNugetPath = currentSearchDirectory
                    .EnumerateDirectories()
                    .Where(d => d.Name == packagesDirectoryName)
                    .Select(SearchPackageDirectoryForNuGet)
                    .FirstOrDefault(x => x != null);
                if (localNugetPath != null)
                    return localNugetPath;

                currentSearchDirectory = currentSearchDirectory.Parent;
            }

            return null;
        }

        private string SearchPackageDirectoryForNuGet(DirectoryInfo packageDirectory)
        {
            const string nugetCliFolderName = "NuGet.CommandLine.*";
            const string localNuGetPath = "tools/NuGet.exe";
            foreach (var result in packageDirectory.EnumerateDirectories(nugetCliFolderName, SearchOption.TopDirectoryOnly))
            {
                var expectedPath = Path.Combine(result.FullName, localNuGetPath);
                if (File.Exists(expectedPath))
                    return expectedPath;
            }
            return null;
        }

    }
}

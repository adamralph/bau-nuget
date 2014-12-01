// <copyright file="NuGetFileFinder.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class NuGetFileFinder
    {
        internal static readonly string defaultNuGetExeName = "NuGet.exe";

        public static FileInfo FindFile()
        {
            var file = FindFiles(new DirectoryInfo(Directory.GetCurrentDirectory())).FirstOrDefault();
            if (file != null)
            {
                return file;
            }

            throw new InvalidOperationException("Failed to find NuGet.exe.");
        }

        private static IEnumerable<FileInfo> FindFiles(DirectoryInfo directory)
        {
            return directory == null
                ? Enumerable.Empty<FileInfo>()
                : directory.EnumerateDirectories("*packages")
                    .SelectMany(packageDirectory => packageDirectory
                        .EnumerateFiles(defaultNuGetExeName, SearchOption.AllDirectories))
                    .Concat(FindFiles(directory.Parent));
        }
    }
}

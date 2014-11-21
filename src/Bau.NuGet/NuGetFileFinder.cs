// <copyright file="NuGetFileFinder.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class NuGetFileFinder
    {
        public FileInfo FindFile()
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
                        .EnumerateFiles("NuGet.exe", SearchOption.AllDirectories))
                    .Concat(FindFiles(directory.Parent));
        }
    }
}

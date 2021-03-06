﻿// <copyright file="NuGetExeFinder.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class NuGetExeFinder
    {
        public static string FindExe()
        {
            var file = FindFiles(new DirectoryInfo(Directory.GetCurrentDirectory())).FirstOrDefault();
            if (file != null)
            {
                return file.FullName;
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

// <copyright file="NuGetTaskExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;

    public static class NuGetTaskExtensions
    {
        public static T Files<T>(this T task, params string[] files) where T : NuGetTask
        {
            task.Files = files;
            return task;
        }

        public static T Files<T>(this T task, IEnumerable<string> files) where T : NuGetTask
        {
            task.Files = files;
            return task;
        }

        public static T In<T>(this T task, string workingDirectory) where T : NuGetTask
        {
            task.WorkingDirectory = workingDirectory;
            return task;
        }

        public static T Use<T>(this T task, string exe) where T : NuGetTask
        {
            task.Exe = exe;
            return task;
        }

        public static T Verbosity<T>(this T task, NuGetVerbosity? verbosity) where T : NuGetTask
        {
            task.Verbosity = verbosity;
            return task;
        }

        public static T VerbosityDetailed<T>(this T task) where T : NuGetTask
        {
            return task.Verbosity(NuGetVerbosity.Detailed);
        }

        public static T VerbosityQuiet<T>(this T task) where T : NuGetTask
        {
            return task.Verbosity(NuGetVerbosity.Quiet);
        }

        public static T VerbosityNormal<T>(this T task) where T : NuGetTask
        {
            return task.Verbosity(NuGetVerbosity.Normal);
        }

        public static T ConfigFile<T>(this T task, string path) where T : NuGetTask
        {
            task.ConfigFile = path;
            return task;
        }
    }
}

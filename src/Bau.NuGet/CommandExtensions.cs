// <copyright file="CommandExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    public static class CommandExtensions
    {
        public static T In<T>(this T command, string workingDirectory) where T : CommandTask
        {
            command.WorkingDirectory = workingDirectory;
            return command;
        }

        public static T Use<T>(this T command, string path) where T : CommandTask
        {
            command.NuGetExePathOverride = path;
            return command;
        }

        public static T Verbosity<T>(this T command, NuGetVerbosity? verbosity) where T : CommandTask
        {
            command.Verbosity = verbosity;
            return command;
        }

        public static T VerbosityDetailed<T>(this T command) where T : CommandTask
        {
            return command.Verbosity(NuGetVerbosity.Detailed);
        }

        public static T VerbosityQuiet<T>(this T command) where T : CommandTask
        {
            return command.Verbosity(NuGetVerbosity.Quiet);
        }

        public static T VerbosityNormal<T>(this T command) where T : CommandTask
        {
            return command.Verbosity(NuGetVerbosity.Normal);
        }

        public static T ConfigFile<T>(this T command, string configFilePath) where T : CommandTask
        {
            command.ConfigFile = configFilePath;
            return command;
        }
    }
}

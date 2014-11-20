// <copyright file="NuGetRequestExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class NuGetRequestExtensions
    {
        public static T WithWorkingDirectory<T>(this T request, string workingDirectory) where T : NuGetRequestBase
        {
            request.WorkingDirectory = workingDirectory;
            return request;
        }

        public static T WithNuGetExePathOverride<T>(this T request, string nugetExePath) where T : NuGetRequestBase
        {
            request.NuGetExePathOverride = nugetExePath;
            return request;
        }

        public static T WithVerbosity<T>(this T request, string verbosity) where T : NuGetRequestBase
        {
            request.Verbosity = verbosity;
            return request;
        }

        public static T WithVerbosityDetailed<T>(this T request) where T : NuGetRequestBase
        {
            return request.WithVerbosity("detailed");
        }

        public static T WithVerbosityQuiet<T>(this T request) where T : NuGetRequestBase
        {
            return request.WithVerbosity("quiet");
        }

        public static T WithVerbosityNormal<T>(this T request) where T : NuGetRequestBase
        {
            return request.WithVerbosity("normal");
        }

        public static T WithConfigFile<T>(this T request, string configFilePath) where T : NuGetRequestBase
        {
            request.ConfigFile = configFilePath;
            return request;
        }
    }
}

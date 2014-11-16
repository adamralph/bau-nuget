// <copyright file="NuGetCliExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class NuGetCliExtensions
    {
        public static T WithVerbosity<T>(this T request, string verbosity) where T : NuGetCliCommandRequestBase
        {
            request.Verbosity = verbosity;
            return request;
        }

        public static T WithVerbosityDetailed<T>(this T request) where T : NuGetCliCommandRequestBase
        {
            return request.WithVerbosity("detailed");
        }

        public static T WithVerbosityQuiet<T>(this T request) where T : NuGetCliCommandRequestBase
        {
            return request.WithVerbosity("quiet");
        }

        public static T WithVerbosityNormal<T>(this T request) where T : NuGetCliCommandRequestBase
        {
            return request.WithVerbosity("normal");
        }

        public static T NonInteractive<T>(this T request, bool enabled = true) where T : NuGetCliCommandRequestBase
        {
            request.NonInteractive = enabled;
            return request;
        }

        public static T WithConfigFile<T>(this T request, string configFilePath) where T : NuGetCliCommandRequestBase
        {
            request.ConfigFile = configFilePath;
            return request;
        }

        public static T WithSource<T>(this T request, string source) where T : NuGetCliDownloadCommandRequestBase
        {
            request.AddSource(source);
            return request;
        }

        public static T WithNoCache<T>(this T request, bool enabled = true) where T : NuGetCliDownloadCommandRequestBase
        {
            request.NoCache = enabled;
            return request;
        }

        public static T WithDisableParallelProcessing<T>(this T request, bool enabled = true) where T : NuGetCliDownloadCommandRequestBase
        {
            request.DisableParallelProcessing = enabled;
            return request;
        }
    }
}

// <copyright file="NuGetCliPushCommandRequest.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NuGetCliPushCommandRequest : NuGetCliCommandRequestBase
    {
        public string TargetPackage { get; set; }

        public string Source { get; set; }

        public string ApiKey { get; set; }

        public int? Timeout { get; set; }

        public bool DisableBuffering { get; set; }

        public NuGetCliPushCommandRequest For(string targetPackage)
        {
            this.TargetPackage = targetPackage;
            return this;
        }

        public NuGetCliPushCommandRequest WithSource(string source)
        {
            this.Source = source;
            return this;
        }

        public NuGetCliPushCommandRequest WithApiKey(string apiKey)
        {
            this.ApiKey = apiKey;
            return this;
        }

        public NuGetCliPushCommandRequest WithTimeout(int? timeout)
        {
            this.Timeout = timeout;
            return this;
        }

        public NuGetCliPushCommandRequest WithDisableBuffering(bool enabled = true)
        {
            this.DisableBuffering = enabled;
            return this;
        }

        public override List<string> CreateCommandLineArguments()
        {
            var arguments = new List<string> { "push" };

            if (!string.IsNullOrWhiteSpace(this.TargetPackage))
            {
                arguments.Add(this.QuoteWrapCliValue(this.TargetPackage));
            }

            if (!string.IsNullOrWhiteSpace(this.ApiKey))
            {
                arguments.Add(this.ApiKey);
            }

            if (this.Timeout.HasValue)
            {
                arguments.Add("-Timeout " + this.Timeout.Value);
            }

            if (this.DisableBuffering)
            {
                arguments.Add("-DisableBuffering");
            }

            if (!string.IsNullOrWhiteSpace(this.Source))
            {
                arguments.Add("-Source " + this.QuoteWrapCliValue(this.Source));
            }

            arguments.AddRange(base.CreateCommandLineArguments());

            return arguments;
        }
    }
}

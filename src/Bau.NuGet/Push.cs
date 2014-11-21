// <copyright file="Push.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;

    public class Push : Command
    {
        public string TargetPackage { get; set; }

        public string Source { get; set; }

        public string ApiKey { get; set; }

        public int? Timeout { get; set; }

        public bool DisableBuffering { get; set; }

        public Push For(string targetPackage)
        {
            this.TargetPackage = targetPackage;
            return this;
        }

        public Push WithSource(string source)
        {
            this.Source = source;
            return this;
        }

        public Push WithApiKey(string apiKey)
        {
            this.ApiKey = apiKey;
            return this;
        }

        public Push WithTimeout(int? timeout)
        {
            this.Timeout = timeout;
            return this;
        }

        public Push WithDisableBuffering(bool enabled = true)
        {
            this.DisableBuffering = enabled;
            return this;
        }

        protected override IEnumerable<string> CreateCommandLineArguments()
        {
            yield return "push";

            if (!string.IsNullOrWhiteSpace(this.TargetPackage))
            {
                yield return this.QuoteWrapCliValue(this.TargetPackage);
            }

            if (!string.IsNullOrWhiteSpace(this.ApiKey))
            {
                yield return this.ApiKey;
            }

            if (this.Timeout.HasValue)
            {
                yield return "-Timeout " + this.Timeout.Value;
            }

            if (this.DisableBuffering)
            {
                yield return "-DisableBuffering";
            }

            if (!string.IsNullOrWhiteSpace(this.Source))
            {
                yield return "-Source " + this.QuoteWrapCliValue(this.Source);
            }
        }
    }
}

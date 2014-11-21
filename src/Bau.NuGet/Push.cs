// <copyright file="Push.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;
    using System.Globalization;

    public class Push : Command
    {
        public string Package { get; set; }

        public string Source { get; set; }

        public string ApiKey { get; set; }

        public int? Timeout { get; set; }

        public bool DisableBuffering { get; set; }

        public Push For(string package)
        {
            this.Package = package;
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

        protected override IEnumerable<string> CreateCustomCommandLineArguments()
        {
            yield return "push";

            if (this.Package != null)
            {
                yield return this.QuoteWrapCliValue(this.Package);
            }

            if (this.Source != null)
            {
                yield return "-Source " + this.QuoteWrapCliValue(this.Source);
            }

            if (this.ApiKey != null)
            {
                yield return this.ApiKey;
            }

            if (this.Timeout.HasValue)
            {
                yield return "-Timeout " + this.Timeout.Value.ToString(CultureInfo.InvariantCulture);
            }

            if (this.DisableBuffering)
            {
                yield return "-DisableBuffering";
            }
        }
    }
}

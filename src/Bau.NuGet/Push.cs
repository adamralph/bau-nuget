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

        public string PackageSource { get; set; }

        public string ApiKey { get; set; }

        public int? TimeoutValue { get; set; }

        public bool BufferingDisabled { get; set; }

        public Push File(string package)
        {
            this.Package = package;
            return this;
        }

        public Push Source(string source)
        {
            this.PackageSource = source;
            return this;
        }

        public Push Key(string apiKey)
        {
            this.ApiKey = apiKey;
            return this;
        }

        public Push Timeout(int? timeout)
        {
            this.TimeoutValue = timeout;
            return this;
        }

        public Push DisableBuffering(bool enabled = true)
        {
            this.BufferingDisabled = enabled;
            return this;
        }

        protected override IEnumerable<string> CreateCustomCommandLineArguments()
        {
            yield return "push";

            if (this.Package != null)
            {
                yield return Command.EncodeArgumentValue(this.Package);
            }

            if (this.PackageSource != null)
            {
                yield return "-Source " + Command.EncodeArgumentValue(this.PackageSource);
            }

            if (this.ApiKey != null)
            {
                yield return this.ApiKey;
            }

            if (this.TimeoutValue.HasValue)
            {
                yield return "-Timeout " + this.TimeoutValue.Value.ToString(CultureInfo.InvariantCulture);
            }

            if (this.BufferingDisabled)
            {
                yield return "-DisableBuffering";
            }
        }
    }
}

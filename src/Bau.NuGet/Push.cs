// <copyright file="Push.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;
    using System.Globalization;

    public class Push : NuGetTask
    {
        private readonly List<string> packages = new List<string>();

        public ICollection<string> Packages
        {
            get { return this.packages; }
        }

        public string PackageSource { get; set; }

        public string ApiKey { get; set; }

        public int? TimeoutValue { get; set; }

        public bool BufferingDisabled { get; set; }

        protected override string Command
        {
            get { return "push"; }
        }

        public Push Files(params string[] packages)
        {
            this.packages.AddRange(packages);
            return this;
        }

        public Push Files(IEnumerable<string> packages)
        {
            this.packages.AddRange(packages);
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

        protected override IEnumerable<string> CreateCustomCommandLineOptions()
        {
            if (this.PackageSource != null)
            {
                yield return "-Source " + NuGetTask.EncodeArgumentValue(this.PackageSource);
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

        protected override IEnumerable<string> GetTargetFiles()
        {
            return this.Packages;
        }
    }
}

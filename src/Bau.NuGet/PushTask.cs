// <copyright file="PushTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System.Collections.Generic;
    using System.Globalization;

    public class PushTask : CommandTask
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

        protected override string OperationName
        {
            get { return "push"; }
        }

        public PushTask Files(params string[] packages)
        {
            this.packages.AddRange(packages);
            return this;
        }

        public PushTask Files(IEnumerable<string> packages)
        {
            this.packages.AddRange(packages);
            return this;
        }

        public PushTask Source(string source)
        {
            this.PackageSource = source;
            return this;
        }

        public PushTask Key(string apiKey)
        {
            this.ApiKey = apiKey;
            return this;
        }

        public PushTask Timeout(int? timeout)
        {
            this.TimeoutValue = timeout;
            return this;
        }

        public PushTask DisableBuffering(bool enabled = true)
        {
            this.BufferingDisabled = enabled;
            return this;
        }

        protected override IEnumerable<string> CreateCustomCommandLineOptions()
        {
            if (this.PackageSource != null)
            {
                yield return "-Source " + CommandTask.EncodeArgumentValue(this.PackageSource);
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

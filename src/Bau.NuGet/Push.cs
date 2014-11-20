﻿// <copyright file="Push.cs" company="Bau contributors">
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
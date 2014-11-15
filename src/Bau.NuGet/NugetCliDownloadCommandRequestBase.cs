// <copyright file="NuGetCliDownloadCommandRequestBase.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;

    public abstract class NuGetCliDownloadCommandRequestBase : NuGetCliCommandRequestBase
    {
        protected NuGetCliDownloadCommandRequestBase()
        {
            this.Source = new List<string>();
        }

        public List<string> Source { get; private set; }

        public bool NoCache { get; set; }

        public bool DisableParallelProcessing { get; set; }

        public string PackageSaveMode { get; set; }

        public override void Apply(object command)
        {
            base.Apply(command);
            ReflectionHelpers.SetInstanceProperty(command, "NoCache", this.NoCache);
            ReflectionHelpers.SetInstanceProperty(command, "DisableParallelProcessing", this.DisableParallelProcessing);
            ReflectionHelpers.SetInstanceProperty(command, "PackageSaveMode", this.PackageSaveMode);
            var commandSource = ReflectionHelpers.GetInstanceProperty(command, "Source") as ICollection<string>;
            if (commandSource != null)
            {
                commandSource.Clear();
                foreach (var sourceItem in this.Source)
                {
                    commandSource.Add(sourceItem);
                }
            }
        }
    }
}

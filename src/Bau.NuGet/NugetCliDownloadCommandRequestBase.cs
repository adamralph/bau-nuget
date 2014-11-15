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
            throw new NotImplementedException();
        }
    }
}

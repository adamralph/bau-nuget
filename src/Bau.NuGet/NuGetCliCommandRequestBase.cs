// <copyright file="NuGetCliCommandRequestBase.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;

    public abstract class NuGetCliCommandRequestBase
    {
        protected NuGetCliCommandRequestBase()
        {
            this.NonInteractive = true;
        }

        public string Verbosity { get; set; }

        public bool NonInteractive { get; set; }

        public string ConfigFile { get; set; }

        public virtual void Apply(object command)
        {
            throw new NotImplementedException();
        }
    }
}

// <copyright file="NuGetCliBauTaskBase.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BauCore;

    public abstract class NuGetCliBauTaskBase : BauTask
    {
        protected NuGetCliBauTaskBase()
        {
            this.UseCommandLine = true;
            this.WorkingDirectory = null;
        }

        public bool UseCommandLine { get; set; }

        public string WorkingDirectory { get; set; }
    }
}

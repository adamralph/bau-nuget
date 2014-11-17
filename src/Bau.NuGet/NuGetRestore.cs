// <copyright file="NuGetRestore.cs" company="Bau contributors">
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

    public class NuGetRestore : NuGetBauTaskBase
    {
        public NuGetRestore()
        {
            this.UseCommandLine = true;
            this.Request = new NuGetCliRestoreCommandRequest();
        }

        public NuGetCliRestoreCommandRequest Request { get; private set; }

        public NuGetCliRestoreCommandRequest For(string targetSolutionOrPackagesConfig)
        {
            return this.Request.For(targetSolutionOrPackagesConfig);
        }

        protected override void OnActionsExecuted()
        {
            if (this.UseCommandLine)
            {
                this.ExecuteBasicUsingCommandLine("restore", this.Request);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}

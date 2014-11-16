// <copyright file="NuGetRestore.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BauCore;

    public class NuGetRestore : NuGetBauTaskBase
    {
        public NuGetRestore()
        {
            this.Request = new NuGetCliRestoreCommandRequest();
        }

        public NuGetCliRestoreCommandRequest Request { get; set; }

        protected override void OnActionsExecuted()
        {
            if (this.Request == null)
            {
                throw new InvalidOperationException("Request is required.");
            }

            NuGetBauTaskBase.CliReflectionWrapper.Restore(this.Request);
        }
    }
}

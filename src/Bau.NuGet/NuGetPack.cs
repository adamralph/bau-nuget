// <copyright file="NuGetPack.cs" company="Bau contributors">
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
    using NuGet;

    public class NuGetPack : NuGetCliBauTaskBase<NuGetCliPackCommandRequest>
    {
        public NuGetCliPackCommandRequest For(string targetProjectOrNuSpec)
        {
            return this.Request.For(targetProjectOrNuSpec);
        }

        protected override void OnActionsExecuted()
        {
            if (this.UseCommandLine)
            {
                this.ExecuteUsingCommandLine();
            }
            else
            {
                var coreRunner = new NuGetPackCoreRunner(this.Request)
                    .WithWorkingDirectory(this.WorkingDirectory);
                coreRunner.Execute();
            }
        }
    }
}

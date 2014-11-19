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

    public class NuGetRestore : NuGetCliBauTaskBase<NuGetCliRestoreCommandRequest>
    {
        public NuGetCliRestoreCommandRequest For(string targetSolutionOrPackagesConfig)
        {
            return this.Request.For(targetSolutionOrPackagesConfig);
        }
    }
}

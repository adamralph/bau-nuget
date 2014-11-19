// <copyright file="NuGetPush.cs" company="Bau contributors">
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

    public class NuGetPush : NuGetCliBauTaskBase<NuGetCliPushCommandRequest>
    {
        public NuGetCliPushCommandRequest For(string targetPackage)
        {
            return this.Request.For(targetPackage);
        }
    }
}

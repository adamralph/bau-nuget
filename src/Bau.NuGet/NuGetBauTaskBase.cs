// <copyright file="NuGetBauTaskBase.cs" company="Bau contributors">
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

    public abstract class NuGetBauTaskBase : BauTask
    {
        private static readonly NuGetCliLoader cliLoader;

        static NuGetBauTaskBase()
        {
            cliLoader = new NuGetCliLoader();
        }
    }
}

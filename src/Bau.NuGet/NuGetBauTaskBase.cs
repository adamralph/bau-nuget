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
        static NuGetBauTaskBase()
        {
            CliLoader = new NuGetCliLoader();
            if (CliLoader.Assembly != null)
            {
                CliReflectionWrapper = new NuGetCliReflectionWrapper(CliLoader.Assembly);
            }
        }

        public static NuGetCliLoader CliLoader { get; private set; }

        public static NuGetCliReflectionWrapper CliReflectionWrapper { get; private set; }
    }
}

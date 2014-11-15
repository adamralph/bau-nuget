// <copyright file="NuGetCliRestoreCommandRequest.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;

    public class NuGetCliRestoreCommandRequest : NuGetCliDownloadCommandRequestBase
    {
        public bool RequireConsent { get; set; }

        public string PackagesDirectory { get; set; }

        public string SolutionDirectory { get; set; }

        public override void Apply(object command)
        {
            base.Apply(command);
            throw new NotImplementedException();
        }
    }
}

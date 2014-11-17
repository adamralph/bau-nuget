// <copyright file="NuGetCliPackCommandPropertyProvider.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NuGet;

    public class NuGetCliPackCommandPropertyProvider : IPropertyProvider
    {
        public NuGetCliPackCommandPropertyProvider(NuGetCliPackCommandRequest request)
        {
            Guard.AgainstNullArgument("request", request);
            this.Request = request;
        }

        public NuGetCliPackCommandRequest Request { get; private set; }

        public dynamic GetPropertyValue(string propertyName)
        {
            if ("VERSION".Equals(propertyName, StringComparison.OrdinalIgnoreCase) 
                && !string.IsNullOrWhiteSpace(this.Request.Version))
            {
                return this.Request.Version;
            }

            string result = null;
            if (this.Request.Properties != null)
            {
                this.Request.Properties.TryGetValue(propertyName, out result);
            }

            return result;
        }
    }
}

// <copyright file="NuGetCliCommandRequestBase.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;

    public abstract class NuGetCliCommandRequestBase
    {
        protected NuGetCliCommandRequestBase()
        {
            this.NonInteractive = true;
        }

        public string Verbosity { get; set; }

        public bool NonInteractive { get; set; }

        public string ConfigFile { get; set; }

        public virtual void Apply(object command)
        {
            Guard.AgainstNullArgument("command", command);
            ReflectionHelpers.SetInstanceProperty(command, "ConfigFile", this.ConfigFile);
            ReflectionHelpers.SetInstanceProperty(command, "NonInteractive", this.NonInteractive);
            var verbosityProperty = command.GetType().GetProperty("Verbosity");
            var verbosityEnumValue = ReflectionHelpers.ConvertToEnumOrDefault(verbosityProperty.PropertyType, this.Verbosity);
            ReflectionHelpers.SetInstanceProperty(command, verbosityProperty, verbosityEnumValue);
        }
    }
}

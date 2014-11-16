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

    public class NuGetPack : NuGetBauTaskBase
    {
        public NuGetPack()
        {
            this.UseCommandLine = false;
            this.Request = new NuGetCliPackCommandRequest();
        }

        public NuGetCliPackCommandRequest Request { get; set; }

        public NuGetCliPackCommandRequest For(string targetProjectOrNuSpec)
        {
            return this.Request.For(targetProjectOrNuSpec);
        }

        protected override void OnActionsExecuted()
        {
            if (this.Request == null)
            {
                throw new InvalidOperationException();
            }

            if (this.UseCommandLine)
            {
                this.ExecuteUsingCommandLine();
            }
            else
            {
                this.ExecuteUsingCore();
            }
        }

        private void ExecuteUsingCommandLine()
        {
            var commandLineArguments = new List<string> { "pack" };
            this.Request.AppendCommandLineOptions(commandLineArguments);

            var execTask = new BauExec.Exec();
            execTask.Command = NuGetBauTaskBase.CliLocator.GetNugetCommandLineAssemblyPath().FullName;
            execTask.Args = commandLineArguments;
            execTask.WorkingDirectory = !string.IsNullOrWhiteSpace(this.WorkingDirectory)
                ? this.WorkingDirectory
                : Directory.GetCurrentDirectory();
            execTask.Execute();
        }

        private void ExecuteUsingCore()
        {
            if(string.IsNullOrWhiteSpace(this.Request.TargetProjectOrNuSpec))
            {
                throw new InvalidOperationException();
            }

            IPropertyProvider propertyProvider = null; // TODO
            var packageBuilder = new PackageBuilder(
                this.Request.TargetProjectOrNuSpec,
                this.Request.BasePath,
                propertyProvider,
                !this.Request.ExcludeEmptyDirectories);

            throw new NotImplementedException();
        }
    }
}

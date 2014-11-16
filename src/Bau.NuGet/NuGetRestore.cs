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

    public class NuGetRestore : NuGetBauTaskBase
    {
        public NuGetRestore()
        {
            this.UseCommandLine = true;
            this.Request = new NuGetCliRestoreCommandRequest();
        }

        public NuGetCliRestoreCommandRequest Request { get; set; }

        public NuGetCliRestoreCommandRequest For(string targetSolutionOrPackagesConfig)
        {
            return this.Request.For(targetSolutionOrPackagesConfig);
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
                throw new NotSupportedException();
            }
        }

        private void ExecuteUsingCommandLine()
        {
            var commandLineArguments = new List<string> { "restore" };
            this.Request.AppendCommandLineOptions(commandLineArguments);

            var execTask = new BauExec.Exec();
            execTask.Command = NuGetBauTaskBase.CliLocator.GetNugetCommandLineAssemblyPath().FullName;
            execTask.Args = commandLineArguments;
            execTask.WorkingDirectory = !string.IsNullOrWhiteSpace(this.WorkingDirectory)
                ? this.WorkingDirectory
                : Directory.GetCurrentDirectory();
            execTask.Execute();
        }
    }
}

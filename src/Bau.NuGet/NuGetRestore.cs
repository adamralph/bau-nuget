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

        public virtual NuGetRestore For(string solutionOrPackagesFile)
        {
            this.Request.TargetSolutionOrPackagesConfig = solutionOrPackagesFile;
            return this;
        }

        public virtual NuGetRestore WithSolutionDirectory(string solutionDirectory)
        {
            this.Request.SolutionDirectory = solutionDirectory;
            return this;
        }

        public virtual NuGetRestore WithPackagesDirectory(string packagesDirectory)
        {
            this.Request.PackagesDirectory = packagesDirectory;
            return this;
        }

        public virtual NuGetRestore InWorkingDirectory(string workingDirectory)
        {
            this.WorkingDirectory = workingDirectory;
            return this;
        }

        protected override void OnActionsExecuted()
        {
            if (this.Request == null)
            {
                throw new InvalidOperationException("Request is required.");
            }

            if (this.UseCommandLine)
            {
                this.ExecuteRestoreUsingCommandLine();
            }
            else
            {
                throw new NotSupportedException("Restore can only be executed using the command line tool.");
            }
        }

        private void ExecuteRestoreUsingCommandLine()
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

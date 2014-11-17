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

    public class NuGetPush : NuGetBauTaskBase
    {
        public NuGetPush()
        {
            this.UseCommandLine = true;
            this.Request = new NuGetCliPushCommandRequest();
        }

        public NuGetCliPushCommandRequest Request { get; private set; }

        public NuGetCliPushCommandRequest For(string targetPackage)
        {
            return this.Request.For(targetPackage);
        }

        protected override void OnActionsExecuted()
        {            
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
            var commandLineArguments = new List<string> { "push" };
            this.Request.AppendCommandLineOptions(commandLineArguments);

            var execTask = new BauExec.Exec();
            execTask.Command = NuGetBauTaskBase.CliLocator.GetNugetCommandLineAssemblyPath().FullName;
            execTask.Args = commandLineArguments;
            execTask.WorkingDirectory = !string.IsNullOrWhiteSpace(this.WorkingDirectory)
                ? Path.GetFullPath(this.WorkingDirectory)
                : Directory.GetCurrentDirectory();
            execTask.Execute();
        }
    }
}

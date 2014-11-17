// <copyright file="NuGetBauTaskBase.cs" company="Bau contributors">
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

    public abstract class NuGetBauTaskBase : BauTask
    {
        static NuGetBauTaskBase()
        {
            CliLocator = new NuGetCliLocator();
        }

        public static NuGetCliLocator CliLocator { get; private set; }

        public bool UseCommandLine { get; set; }

        public string WorkingDirectory { get; set; }

        protected virtual void ExecuteBasicUsingCommandLine(string operationName, NuGetCliCommandRequestBase request)
        {
            var commandLineArguments = new List<string> { operationName };
            request.AppendCommandLineOptions(commandLineArguments);

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

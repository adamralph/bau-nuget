// <copyright file="NuGetBasicCommandLineRunner.cs" company="Bau contributors">
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

    public class NuGetBasicCommandLineRunner
    {
        public string WorkingDirectory { get; set; }

        public NuGetBasicCommandLineRunner WithWorkingDirectory(string workingDirectory)
        {
            this.WorkingDirectory = workingDirectory;
            return this;
        }

        public virtual void Execute(NuGetCliCommandRequestBase request)
        {
            var execTask = new BauExec.Exec();
            execTask.Command = NuGetCliLocator.Default.GetNugetCommandLineAssemblyPath().FullName;
            execTask.Args = request.CreateCommandLineArguments();
            execTask.WorkingDirectory = !string.IsNullOrWhiteSpace(this.WorkingDirectory)
                ? Path.GetFullPath(this.WorkingDirectory)
                : Directory.GetCurrentDirectory();
            execTask.Execute();
        }
    }
}

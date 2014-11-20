// <copyright file="NuGetBasicCommandLineRunner.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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

        public virtual ProcessStartInfo CreateProcessStartInfo(NuGetCliCommandRequestBase request)
        {
            return new ProcessStartInfo
            {
                FileName = NuGetCliLocator.Default.GetNugetCommandLineAssemblyPath().FullName,
                Arguments = string.Join(" ", request.CreateCommandLineArguments()),
                WorkingDirectory = !string.IsNullOrWhiteSpace(this.WorkingDirectory)
                    ? Path.GetFullPath(this.WorkingDirectory)
                    : Directory.GetCurrentDirectory(),
                UseShellExecute = false
            };
        }
    }
}

// <copyright file="NuGetPackCoreRunner.cs" company="Bau contributors">
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
    using NuGet;

    public class NuGetPackCoreRunner
    {
        public NuGetPackCoreRunner(NuGetCliPackCommandRequest request)
        {
            Guard.AgainstNullArgument("request", request);
            this.Request = request;
        }

        public NuGetCliPackCommandRequest Request { get; private set; }

        public string WorkingDirectory { get; set; }

        public NuGetPackCoreRunner WithWorkingDirectory(string workingDirectory)
        {
            this.WorkingDirectory = workingDirectory;
            return this;
        }

        public void Execute()
        {
            if (string.IsNullOrWhiteSpace(this.Request.TargetProjectOrNuSpec))
            {
                throw new InvalidOperationException();
            }

            if (this.Request.Build)
            {
                throw new NotImplementedException();
            }

            if (this.Request.Exclude != null && this.Request.Exclude.Any())
            {
                throw new NotImplementedException();
            }

            if (this.Request.Symbols)
            {
                throw new NotImplementedException();
            }

            if (this.Request.Tool)
            {
                throw new NotImplementedException();
            }

            if (this.Request.NoDefaultExcludes)
            {
                throw new NotImplementedException();
            }

            if (this.Request.IncludeReferencedProjects)
            {
                throw new NotImplementedException();
            }

            if (!string.IsNullOrWhiteSpace(this.Request.MiniClientVersion))
            {
                throw new NotImplementedException();
            }

            if (!this.Request.NoPackageAnalysis)
            {
                throw new NotImplementedException();
            }

            if (".NUSPEC".Equals(Path.GetExtension(this.Request.TargetProjectOrNuSpec), StringComparison.OrdinalIgnoreCase))
            {
                this.ExecuteForNuSpec();
            }
            else
            {
                this.ExecuteForProject();
            }
        }

        protected void ExecuteForNuSpec()
        {
            var basePath = string.IsNullOrWhiteSpace(this.Request.BasePath)
                ? Path.GetDirectoryName(this.Request.TargetProjectOrNuSpec)
                : this.Request.BasePath;
            var propertyProvider = new NuGetCliPackCommandPropertyProvider(this.Request);
            var packageBuilder = new PackageBuilder(
                this.Request.TargetProjectOrNuSpec,
                basePath,
                propertyProvider,
                !this.Request.ExcludeEmptyDirectories);

            var versionString = this.Request.ExtractVersionString();
            SemanticVersion semVer;
            if (!string.IsNullOrWhiteSpace(versionString) && SemanticVersion.TryParse(versionString, out semVer))
            {
                packageBuilder.Version = semVer;
            }

            var packageFileName = packageBuilder.Id + "." + packageBuilder.Version.ToString() + ".nupkg";
            string outputDirectory;
            if (!string.IsNullOrWhiteSpace(this.Request.OutputDirectory))
            {
                outputDirectory = this.Request.OutputDirectory;
            }
            else if (!string.IsNullOrWhiteSpace(this.WorkingDirectory))
            {
                outputDirectory = this.WorkingDirectory;
            }
            else
            {
                outputDirectory = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var packageOutFilePath = Path.Combine(outputDirectory, packageFileName);
            try
            {
                using (var fileStream = File.Create(packageOutFilePath))
                {
                    packageBuilder.Save(fileStream);
                }
            }
            catch
            {
                if (File.Exists(packageOutFilePath))
                {
                    File.Delete(packageOutFilePath);
                }

                throw;
            }
        }

        protected void ExecuteForProject()
        {
            throw new NotImplementedException();
        }
    }
}

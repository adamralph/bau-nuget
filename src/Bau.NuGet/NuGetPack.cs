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
            this.UseCommandLine = true;
            this.Request = new NuGetCliPackCommandRequest();
        }

        public NuGetCliPackCommandRequest Request { get; private set; }

        public NuGetCliPackCommandRequest For(string targetProjectOrNuSpec)
        {
            return this.Request.For(targetProjectOrNuSpec);
        }

        protected override void OnActionsExecuted()
        {
            if (this.UseCommandLine)
            {
                this.ExecuteBasicUsingCommandLine("pack", this.Request);
            }
            else
            {
                this.ExecuteUsingCore();
            }
        }

        private void ExecuteUsingCore()
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
                this.ExecuteUsingCoreForNuSpec();
            }
            else
            {
                this.ExecuteUsingCoreForProject();
            }
        }

        private void ExecuteUsingCoreForNuSpec()
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

        private void ExecuteUsingCoreForProject()
        {
            throw new NotImplementedException();
        }
    }
}

// <copyright file="NuGetPackFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;
    using Xunit.Extensions;

    public static class NuGetPackFacts
    {
        [Fact]
        public static void CanPackPackageUsingCli()
        {
            // arrange
            var task = new NuGetTask();
            var request = task
                .Pack("./pickles.nuspec")
                .WithWorkingDirectory("./")
                .WithVersion("0.1.2-alpha99999")
                .WithOutputDirectory("./packed")
                .WithProperty("Unused", "Don't Care")
                .WithProperty("Authors", "Peter Piper")
                .WithExclude("poo.p");

            NuGetCliLocatorFacts.InstallNuGetCli();

            if (Directory.Exists(request.OutputDirectory))
            {
                Thread.Sleep(500);
                Directory.Delete(request.OutputDirectory, true);
            }

            using (var pickes = File.CreateText(Path.Combine(Path.GetDirectoryName(request.TargetProjectOrNuSpec), "pickles.txt")))
            {
                pickes.WriteLine("Peter Piper picked a peck of pickled peppers.");
            }

            using (var nuspecStream = File.CreateText(request.TargetProjectOrNuSpec))
            using (var xmlWriter = System.Xml.XmlWriter.Create(nuspecStream))
            {
                xmlWriter.WriteStartElement("package", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

                xmlWriter.WriteStartElement("metadata");
                
                xmlWriter.WriteStartElement("id");
                xmlWriter.WriteString("pickles");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("authors");
                xmlWriter.WriteString("$authors$");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("version");
                xmlWriter.WriteString("$version$");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("description");
                xmlWriter.WriteString("A peck of pickles in a package.");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement(); // metadata

                xmlWriter.WriteStartElement("files");

                xmlWriter.WriteStartElement("file");
                xmlWriter.WriteAttributeString("src", "./pickles.txt");
                xmlWriter.WriteAttributeString("target", "readme.txt");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("file");
                xmlWriter.WriteAttributeString("src", "Bau.NuGet.Test.Unit.dll");
                xmlWriter.WriteAttributeString("target", "lib/sl5/peck-of-pickles.dll");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement(); // files

                xmlWriter.WriteEndElement();
            }

            Directory.CreateDirectory(request.OutputDirectory);
            Thread.Sleep(100);

            // act
            task.Execute();

            // assert
            File.Exists(Path.Combine(request.OutputDirectory, "pickles." + request.Version + ".nupkg")).Should().BeTrue();
        }

        [Fact]
        public static void CanCreateMultiplePackRequests()
        {
            // arrange
            var task = new NuGetTask();
            var fakeDirName = "./fake-dir/";

            // act
            task.Pack(
                new[] { "file1", "file2" },
                r => r
                    .WithWorkingDirectory(fakeDirName)
                    .WithTool());

            // assert
            task.Requests.Should().HaveCount(2);
            task.Requests.All(r => r.WorkingDirectory == fakeDirName).Should().BeTrue();
            task.Requests.OfType<NuGetCliPackCommandRequest>().All(r => r.Tool).Should().BeTrue();
            task.Requests.OfType<NuGetCliPackCommandRequest>().Select(x => x.TargetProjectOrNuSpec).Should().Contain("file1");
            task.Requests.OfType<NuGetCliPackCommandRequest>().Select(x => x.TargetProjectOrNuSpec).Should().Contain("file2");
        }
    }
}
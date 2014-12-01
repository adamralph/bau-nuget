// <copyright file="PackFacts.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet.Test.Unit
{
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml;
    using FluentAssertions;
    using Xunit;

    public static class PackFacts
    {
        public static void Packs()
        {
            // arrange
            var task = new NuGetTask();
            var pack = task
                .Pack("./pickles.nuspec")
                .In("./")
                .Version("0.1.2-alpha99999")
                .Output("./packed")
                .Property("Unused", "Don't Care")
                .Property("Authors", "Peter Piper")
                .Exclude("poo.p");

            if (Directory.Exists(pack.OutputDirectory))
            {
                Thread.Sleep(500);
                Directory.Delete(pack.OutputDirectory, true);
            }

            using (var pickes = File.CreateText(
                Path.Combine(Path.GetDirectoryName(pack.NuSpecOrProject), "pickles.txt")))
            {
                pickes.WriteLine("Peter Piper picked a peck of pickled peppers.");
            }

            using (var nuspecStream = File.CreateText(pack.NuSpecOrProject))
            using (var xmlWriter = XmlWriter.Create(nuspecStream))
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

            Directory.CreateDirectory(pack.OutputDirectory);
            Thread.Sleep(100);

            // act
            task.Execute();

            // assert
            File.Exists(Path.Combine(pack.OutputDirectory, "pickles." + pack.VersionValue + ".nupkg")).Should().BeTrue();
        }

        [Fact]
        public static void CreatesMultiplePackCommands()
        {
            // arrange
            var task = new NuGetTask();
            var fakeDirName = "./fake-dir/";

            // act
            task.Pack(
                new[] { "file1", "file2" },
                r => r
                    .In(fakeDirName)
                    .AsTool());

            // assert
            task.Commands.Should().HaveCount(2);
            task.Commands.All(r => r.WorkingDirectory == fakeDirName).Should().BeTrue();
            task.Commands.OfType<PackTask>().All(r => r.Tool).Should().BeTrue();
            task.Commands.OfType<PackTask>().Select(x => x.NuSpecOrProject).Should().Contain("file1");
            task.Commands.OfType<PackTask>().Select(x => x.NuSpecOrProject).Should().Contain("file2");
        }
    }
}
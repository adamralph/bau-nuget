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
            var pack = new Pack()
                .Files("./pickles.nuspec")
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
                Path.Combine(Path.GetDirectoryName(pack.Files.Single()), "pickles.txt")))
            {
                pickes.WriteLine("Peter Piper picked a peck of pickled peppers.");
            }

            using (var nuspecStream = File.CreateText(pack.Files.Single()))
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
            pack.Execute();

            // assert
            File.Exists(Path.Combine(pack.OutputDirectory, "pickles." + pack.VersionValue + ".nupkg")).Should().BeTrue();
        }

        [Fact]
        public static void HasAFluentApi()
        {
            // arrange
            var pack = new Pack();
            var fakeDirName = "./fake-dir/";

            // act
            pack
                .Files("file1", "file2")
                .In(fakeDirName)
                .AsTool();

            // assert
            pack.Files.Should().HaveCount(2);
            pack.Files.Should().Contain("file1");
            pack.Files.Should().Contain("file2");
            pack.WorkingDirectory.Should().Be(fakeDirName);
            pack.Tool.Should().BeTrue();
        }

        [Fact]
        public static void SetPropertiesFromAnonType()
        {
            // arrange
            var pack = new Pack();

            // act
            pack
                .Files("test")
                .Properties(new { A = 1, B = "two", C = 3.1, D = true });

            // assert
            pack.PropertiesCollection.Should().HaveCount(4);
            pack.PropertiesCollection.Should().Contain("A", "1");
            pack.PropertiesCollection.Should().Contain("B", "two");
            pack.PropertiesCollection.Should().Contain("C", "3.1");
            pack.PropertiesCollection.Should().Contain("D", "True");
        }

        [Fact]
        public static void CanHaveExtraArgsAdded()
        {
            // arrange
            var extraArg = "-DoMagicThings";
            var task = new Pack().With(new[] { extraArg });

            // act
            var arguments = task.CreateCommandLineOptions();

            // assert
            arguments.Should().Contain(extraArg);
            task.ExtraArgs.Should().BeEquivalentTo(extraArg);
        }
    }
}
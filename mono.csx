Require<Bau>()

.Task("default").DependsOn("unit")

.Task("logs").Do(() => CreateDirectory("artifacts/logs"))

.Exec("clean").DependsOn("logs").Do(exec => exec
    .Run("xbuild")
    .With("src/Bau.NuGet.sln", "/target:Clean", "/property:Configuration=Release", "/verbosity:normal", "/nologo"))

.Exec("restore").Do(exec => exec
    .Run("mono")
    .With("packages/NuGet.CommandLine.2.8.3/tools/NuGet.exe", "restore", "src/Bau.NuGet.sln"))

.Exec("build").DependsOn("clean", "restore", "logs").Do(exec => exec
    .Run("xbuild")
    .With("src/Bau.NuGet.sln", "/target:Build", "/property:Configuration=Release", "/verbosity:normal", "/nologo"))

.Task("tests").Do(() => CreateDirectory("artifacts/tests"))

.Xunit("unit").DependsOn("build", "tests").Do(xunit => xunit
    .Use("./packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe")
    .Run("./src/test/Bau.NuGet.Test.Unit/bin/Release/Bau.NuGet.Test.Unit.dll")
    .Html().Xml())

.Run();

void CreateDirectory(string name)
{
    if (!Directory.Exists(name))
    {
        Directory.CreateDirectory(name);
        System.Threading.Thread.Sleep(100); // HACK (adamralph): wait for the directory to be created
    }
}

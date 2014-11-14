@ECHO OFF

ECHO [Dog] WARNING: Ensure you have built using Debug config (e.g in Visual Studio) before building with this!

ECHO [Dog] Copying Bau.NuGet from bin\Debug...
xcopy src\Bau.NuGet\bin\Debug\Bau.NuGet.dll packages\Bau.NuGet.0.1.0-adhoc\lib\net45\ /Y /Q
xcopy artifacts\output\Bau.NuGet.0.1.0-adhoc.nupkg packages\Bau.NuGet.0.1.0-adhoc\ /Y /Q

ECHO [Dog] Running dog.csx...
scriptcs dog.csx -- %*

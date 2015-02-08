@ECHO OFF

ECHO [Dog] WARNING: Ensure you have built using Debug config (e.g in Visual Studio) before building with this!

scriptcs -I

ECHO [Dog] Copying Bau.NuGet from bin\Debug...
xcopy src\Bau.NuGet\bin\Debug\Bau.NuGet.dll scriptcs_packages\Bau.NuGet.0.1.0-alpha001\lib\net45\ /Y /Q

ECHO [Dog] Running dog.csx...
scriptcs dog.csx -- %*

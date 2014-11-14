@ECHO OFF

ECHO [Dog] WARNING: Ensure you have built using baufile.csx before building with this!

ECHO [Dog] Copying Bau.MSBuild from bin\Release...
xcopy src\Bau.MSBuild\bin\Release\Bau.NuGet.dll packages\Bau.NuGet.0.1.0-beta01\lib\net45\ /Y /Q

ECHO [Dog] Running dog.csx...
scriptcs dog.csx -- %*

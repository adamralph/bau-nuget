@ECHO OFF

ECHO [Dog] WARNING: Ensure you have built using baufile.csx before building with this!

scriptcs -I

ECHO [Dog] Copying Bau.NuGet from bin\Release...
xcopy src\Bau.NuGet\bin\Release\Bau.NuGet.dll scriptcs_packages\Bau.NuGet.0.1.0-alpha001\lib\net45\ /Y /Q

ECHO [Dog] Running dog.csx...
scriptcs -script dog.csx -- %*

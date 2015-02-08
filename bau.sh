#!/usr/bin/env bash
set -e
set -o pipefail
set -x

# before_install
# sudo bash -c "echo deb http://badgerports.org precise main >> /etc/apt/sources.list"
# sudo apt-key adv --keyserver keyserver.ubuntu.com --recv-keys 0E1FAD0C
# sudo apt-get update -qq
# sudo apt-get install -qq mono-devel
# mozroots --import --sync

# install
if [ ! -d ./scriptcs ]
  then
    rm -f ./ScriptCs.0.13.2.nupkg
    wget "http://chocolatey.org/api/v2/package/ScriptCs/0.13.2" -O ScriptCs.0.13.2.nupkg
    unzip ./ScriptCs.0.13.2.nupkg -d scriptcs
fi

mono ./scriptcs/tools/scriptcs.exe -install
if [ -d ./scriptcs_packages/Bau.XUnit.0.1.0-beta06 ]
  then
    mv ./scriptcs_packages/Bau.XUnit.0.1.0-beta06/Bau.XUnit.0.1.0-beta06.nupkg ./scriptcs_packages/Bau.XUnit.0.1.0-beta06/Bau.Xunit.0.1.0-beta06.nupkg
    mv ./scriptcs_packages/Bau.XUnit.0.1.0-beta06 ./scriptcs_packages/Bau.Xunit.0.1.0-beta06
fi

mono ./scriptcs_packages/NuGet.CommandLine.2.8.3/tools/NuGet.exe restore src/Bau.NuGet.sln
  
# script
mono ./scriptcs/tools/scriptcs.exe ./mono.csx -- $@

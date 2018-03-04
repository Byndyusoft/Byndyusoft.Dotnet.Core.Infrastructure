#!/usr/bin/env bash

SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
TOOLS_DIR=$SCRIPT_DIR/tools
TEMP_DIR=$SCRIPT_DIR/tmp
TEMP_PROJECT=$TEMP_DIR/tmp.csproj

SCRIPT="build.cake"
TARGET="Default"
CONFIGURATION="Release"
VERBOSITY="verbose"
DRYRUN=
SHOW_VERSION=false
SCRIPT_ARGUMENTS=()

for i in "$@"; do
    case $1 in
        -s|--script) SCRIPT="$2"; shift ;;
        -t|--target) TARGET="$2"; shift ;;
        -c|--configuration) CONFIGURATION="$2"; shift ;;
        -v|--verbosity) VERBOSITY="$2"; shift ;;
        -d|--dryrun) DRYRUN="-dryrun" ;;
        --version) SHOW_VERSION=true ;;
        --) shift; SCRIPT_ARGUMENTS+=("$@"); break ;;
        *) SCRIPT_ARGUMENTS+=("$1") ;;
    esac
    shift
done

dotnet new classlib -o "$TEMP_DIR" --no-restore
dotnet add "$TEMP_PROJECT" package --package-directory "$TOOLS_DIR" Cake.CoreCLR
rm -rf tmp
CAKE_PATH=$(find "$TOOLS_DIR" -name Cake.dll | sort -r | head -1)

if $SHOW_VERSION; then
    dotnet "$CAKE_PATH" --version
else
    dotnet "$CAKE_PATH" $SCRIPT --nuget_useinprocessclient=true --settings_skipverification=true --verbosity=$VERBOSITY --configuration=$CONFIGURATION --target=$TARGET $DRYRUN "${SCRIPT_ARGUMENTS[@]}"
fi
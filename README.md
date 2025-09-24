# SortingGame

A mixed _VSCode_ / _Unity_ project.
Shows a couple of sorting algorithms and uses them in plain C# and _Unity_ projects.

## Pre-requisites

- Log in as a user, not _admin_; `dotnet` and `Unity` require admin privileges
- `dotnet` SDK: https://dotnet.microsoft.com/en-us/download
- `Visual Studio Code`, User mode: https://code.visualstudio.com/download
    - `C# Dev Kit` extension when asked
    - `Unity` extension when asked
- `Unity Hub`: https://unity.com/download
    - Create an account
- _Unity editor_:
    - Weaker PC: version _2020.3.48f1_: https://unity.com/releases/editor/archive
          - deselect `Visual Studio 2019`, install `Visual Studio 2022` later if desired
    - Stronger PC: latest version from _Unity Hub_ -> _Installs_ -> _Install Editor_
          - `Visual Studio 2022` recommended
    - Any platforms you wish, check their sizes though.
 
### Optional

- `Blender 3D` for better meshes and rigging
- `Visual Studio 2022` for better debugging experience
- `ProBuilder` package from Unity Registry for mesh modification
- `FBX Exporter` package from Unity Registry (use _binary_ mode) for exporting to `Blender`


## Steps for CLI

### Scafold projects

```
mkdir ...
cd ...
dotnet new sln -n ...
dotnet new classlib -n SortingLib
dotnet sln add SortingLib
dotnet new console -n SortingCli
dotnet sln add SortingCli
code .
```

### Adding packages

```
dotnet add .\SortingCli.csproj reference ../SortingLib/SortingLib.csproj
dotnet add package CommandLineParser
```

### Run

Create a dotnet project execution.

## Steps for Unity

- Use `mklink /D` or `New-Item -ItemType SymbolicLink` to create a link to `SortingLib` under `Assets/External`.
- Delete `bin` and `obj`.

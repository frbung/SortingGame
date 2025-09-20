# SortingGame

A mixed _VSCode_ / _Unity_ project.
Shows a couple of sorting algorithms and uses them in plain C# and _Unity_ projects.

## Pre-requisites

- Visual Studio Code
    - C# Dev
- Unity: https://unity.com/releases/editor/archive, version 2020.3.48f1
    - ProBuilder package


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

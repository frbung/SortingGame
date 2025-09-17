# SortingGame

A mixed _VSCode_ / _Unity_ project.
Shows a couple of sorting algorithms and uses them in plain C# and _Unity_ projects.

## Pre-requisites

- Visual Studio Code
- Unity: https://unity.com/releases/editor/archive, version 2020.3.48f1


## Steps

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

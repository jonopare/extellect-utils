# Extellect Core
This is a dramatically reduced subset of Extellect Utilities built for .NET Core (.NET 8) instead of .NET Framework.

## Nuget
Packaging and publishing:
In Visual Studio click `Tools` > `NuGet Package Manager` > `Package Manager Console`.
In the Package Manager Console window, type: `dotnet pack Extellect.Core`.

`dotnet nuget push Extellect.Core/bin/Release/Extellect.Core.2.0.0.nupkg -s https://api.nuget.org/v3/index.json -k api_key`
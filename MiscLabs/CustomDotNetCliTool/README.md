# .NET Core Global Tools

```powershell
cd StatisticsToolbox
dotnet pack
dotnet tool install --add-source .nuget\ stat --tool-path .\tools
dotnet tool update --add-source .nuget\ stat --tool-path .\tools
.\tools\stat.exe avg  -n 1
.\tools\stat.exe sd  -n 1 2 3
```

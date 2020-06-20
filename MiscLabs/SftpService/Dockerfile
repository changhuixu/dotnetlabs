# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY *.sln .
COPY SftpDemo/*.csproj SftpDemo/
COPY SFTPService/*.csproj SFTPService/
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/SftpDemo
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "SftpDemo.dll"]

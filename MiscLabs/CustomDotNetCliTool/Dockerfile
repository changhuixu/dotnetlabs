# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY *.sln ./
COPY StatisticsToolbox/*.csproj StatisticsToolbox/
COPY SqlConnect/*.csproj SqlConnect/
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/StatisticsToolbox
RUN dotnet pack
RUN dotnet tool install stat --add-source .nuget --tool-path /src/tools
# the Global Tool is in /src/tools folder
WORKDIR /src/tools

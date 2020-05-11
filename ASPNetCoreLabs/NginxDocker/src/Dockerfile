FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

COPY *.sln .
COPY MyWebApi/*.csproj ./MyWebApi/
RUN dotnet restore

COPY MyWebApi/. ./MyWebApi/
WORKDIR /app/MyWebApi
RUN dotnet publish -c Release -o /out --no-restore


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app
COPY --from=build /out ./
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "MyWebApi.dll"]
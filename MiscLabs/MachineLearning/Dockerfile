# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY *.sln ./
COPY HandwritingRecognition/*.csproj HandwritingRecognition/
COPY HandwritingRecognitionML.Model/*.csproj HandwritingRecognitionML.Model/
COPY HandwritingRecognitionML.ConsoleApp/*.csproj HandwritingRecognitionML.ConsoleApp/
RUN dotnet restore 
COPY . .

# Build MLModel.zip
FROM build AS modeling
# WORKDIR /src/HandwritingRecognitionML.ConsoleApp
# RUN dotnet run
WORKDIR /src/HandwritingRecognition
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "HandwritingRecognition.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet HandwritingRecognition.dll
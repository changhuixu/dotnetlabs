# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY *.sln ./
COPY HandwritingRecognition/*.csproj HandwritingRecognition/
COPY HandwritingRecognitionML.Model/*.csproj HandwritingRecognitionML.Model/
COPY HandwritingRecognitionML.ConsoleApp/*.csproj HandwritingRecognitionML.ConsoleApp/
COPY TaxiFareML.Model/*.csproj TaxiFareML.Model/
RUN dotnet restore
COPY . .

# Build MLModel.zip
FROM build AS publish
# WORKDIR /src/HandwritingRecognitionML.ConsoleApp
# RUN dotnet run
WORKDIR /src/HandwritingRecognition
RUN dotnet publish -c Release -o /src/publish
RUN cp ./MLModel.zip /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
    libc6-dev \
    libgdiplus \
    libx11-dev \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "HandwritingRecognition.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet HandwritingRecognition.dll
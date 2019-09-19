# Throttling Concurrent Outgoing HTTP Requests in .NET Core

- A .NET Core Console app with Dependency Injection, HttpClient
- Throttling concurrent outgoing HTTP requests using a `semaphore`

## [Medium Post](https://medium.com/@changhuixu/throttling-concurrent-outgoing-http-requests-in-net-core-404b5acd987b)

## How to run the project

1. Go up one directory to `ThrottledWebApi` project. In a terminal, issue command `dotnet run` to start the Web API application.

2. In this folder, open a new terminal, run command `dotnet run` to check the Console ouput. ThrottledWebApi.ClientDemo

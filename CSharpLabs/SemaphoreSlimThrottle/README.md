# Rate Limiting API Endpoints in ASP.NET Core

## [Medium Post 1: Rate Limiting API Endpoints in ASP.NET Core](https://medium.com/@changhuixu/rate-limiting-api-endpoints-in-asp-net-core-926e31428017)

This post shows (1) an ASP.NET Core Web API demo project which limits inbound HTTP requests from the internet and (2) an integration test project which tests the Web API rate limit using a `TestServer` and an `HttpClient`. One of the integration tests shows an example approach to send concurrent API requests using a `Semaphore` in order to comply with the rate limit in the Web API application.

## [Medium Post 2: Throttling Concurrent Outgoing HTTP Requests in .NET Core](https://medium.com/@changhuixu/throttling-concurrent-outgoing-http-requests-in-net-core-404b5acd987b)

This post will go over how to make concurrent outgoing HTTP requests _on the client side_. The goal is to let the HTTP Client send concurrent requests at the maximum allowed rate which is set by the server, for example, at a maximum rate of 2 requests per second.

## Solution Structure

This solution contains 3 projects.

1 `ThrottledWebApi`

- An ASP.NET Core Web API project
- Contains one API endpoint: `/api/values/isPrime?number={number}`
- The API endpoint is enforced with rate limit

2 `ThrottledWebApi.IntegrationTests`

- An integration test project with an in-memory test server
- An HTTP client is used to test against the API endpoint
- The rate limiting effect in different scenarios are tested

3 `ThrottledWebApi.ClientDemo`

- A .NET Core Console app with Dependency Injection, HttpClient
- Throttling concurrent outgoing HTTP requests using a `semaphore`

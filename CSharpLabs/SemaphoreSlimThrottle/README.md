# Rate Limiting API Endpoints in ASP.NET Core

## [Medium Post 1](https://medium.com/@changhuixu/rate-limiting-api-endpoints-in-asp-net-core-926e31428017)

## [Medium Post 2](https://medium.com/@changhuixu/throttling-concurrent-outgoing-http-requests-in-net-core-404b5acd987b)

### This solution contains 3 projects.

1 ThrottledWebApi

- An ASP.NET Core Web API project
- Contains one API endpoint: `/api/values/isPrime?number={number}`
- The API endpoint is enforced with rate limit

2 ThrottledWebApi.IntegrationTests

- An integration test project with an in-memory test server
- An HTTP client is used to test against the API endpoint
- The rate limiting effect in different scenarios are tested

3 ThrottledWebApi.ClientDemo

- A .NET Core Console app with Dependency Injection, HttpClient
- Throttling concurrent outgoing HTTP requests using a `semaphore`

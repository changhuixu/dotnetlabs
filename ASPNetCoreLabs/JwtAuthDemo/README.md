# API Auth Demo: JWT auth and Basic auth (Repository Archived)

## New Repository ([https://github.com/dotnet-labs/ApiAuthDemo](https://github.com/dotnet-labs/ApiAuthDemo))

This repository demos a Web API project, `ApiAuthDemo`, which is configured to use JWT authentication globally, and some action methods can be configured to use Basic Auth using an attribute. The demo website root is its Swagger user interface.

There is another Console app `BasicAuthApiConsumer`, which is a demo to consume a Basic Authentication API endpoint. The Console app needs to run after you start the `ApiAuthDemo` website, and you can adjust the URL in the Console program accordingly.

## Medium Articles

### [Basic Authentication](https://codeburst.io/adding-basic-authentication-to-an-asp-net-core-web-api-project-5439c4cf78ee)

> How to add Basic Authentication to an ASP.NET Core 3 Web API project.

### [API Security in Swagger](https://codeburst.io/api-security-in-swagger-f2afff82fb8e)

> How to configure security schemes for our API documentation in Swagger.

## Screen recordings

### Screen recording for an API with Basic Authentication

![Screen recording for an API with Basic Authentication](./basic-auth-edge.gif)

### Screen recording for API authentication in Swagger UI

![Screen recording for API authentication in Swagger UI](./swagger-auth.gif)

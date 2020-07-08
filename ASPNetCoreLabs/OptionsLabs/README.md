# Options Pattern in .NET Core

This solution demos one of the usages of the Options Pattern in .NET Core.

## [Medium Article: Options Patter in .NET Core](https://codeburst.io/options-pattern-in-net-core-a50285aeb18d)

When registering dependencies in the `ConfigureServices` method, you must have seen a pattern likes the following

```CSharp
services.AddDbContext<T>(options => options.**)

services.AddSwaggerGen(c => {
    c.SwaggerDoc(**);
})

```

This pattern is actually an extention method on top of `IServiceCollection`, and the naming convension of this type of extension method is `AddSomeService(this IServiceCollection services, Action<SomeOptions> action)`. The `action` is a Lambda function, which can be used to provide extra parameters to the service.

In this blog post, we will create a similar service that can be registered by calling the `services.AddMyService(Action<MyServiceOptions> action)` method. We will pass options to `MyService` so that this service can be more flexible with extra parameters.

## Intro to Options pattern in ASP.NET Core

[Microsoft Doc](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options)

The options pattern uses classes to represent groups of related settings. When configuration settings are isolated by scenario into separate classes, the app adheres to two important software engineering principles:

- The Interface Segregation Principle (ISP) or Encapsulation – Scenarios (classes) that depend on configuration settings depend only on the configuration settings that they use.
- Separation of Concerns – Settings for different parts of the app aren't dependent or coupled to one another.

# Unit Testing with .NET Core `ILogger<T>` (Repository Archived)

## New Repository ([https://github.com/dotnet-labs/UnitTestingWithILogger](https://github.com/dotnet-labs/UnitTestingWithILogger))

## [Medium Article](https://codeburst.io/unit-testing-with-net-core-ilogger-t-e8c16c503a80)

Because `ILogger<T>` objects are frequently used in controllers and service classes, we cannot avoid them in unit tests. In this post, we will go over some common ways to work with `ILogger<T>` objects in unit testing.

1. Replace `ILogger<T>` with `NullLogger<T>`
2. Create a Real `ILogger<T>` that Logs to Console
3. Mock an `ILogger<T>` Object

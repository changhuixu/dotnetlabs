# Run an External Executable in ASP.NET Core

## [Medium Post](https://codeburst.io/run-an-external-executable-in-asp-net-core-5c2f8b6cacd9)

This post will go over an example use case that a `helloworld.exe` (in _Windows_) is executed from an ASP.NET Core Web API endpoint. This solution might need to be slightly modified if your application works in another OS.

## Solution Structure

- `HelloWorld`

  This project creates a contrived program called `helloworld.exe` in .NET 4.6.2 framework or in an older version. This `helloworld.exe` simulates the external program that will be triggered by an ASP.NET Core Web API project.

- `WebApi`

  In the `ValuesController`, an API executes the `helloworld.exe`.

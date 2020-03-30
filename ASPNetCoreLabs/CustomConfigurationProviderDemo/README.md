# Create a Custom Configuration Provider in ASP.NET Core

## [Medium Article](https://codeburst.io/create-a-custom-configuration-provider-in-asp-net-core-cdd6a32b8ecb)

ASP.NET Core offers a native configuration system that is lightweight and highly extensible. We can aggregate configurations using one or more configuration providers, which read configuration key-value pairs from a variety of configuration sources, such as JSON files, environment variables, secret manager, command line arguments, and so on. Moreover, we can use NuGet packages to pull configurations from Azure Key Vault, AWS System Manager, and more.

Custom configuration providers are not uncommon. In many software architectures, configuration sources reside in separate stores in order to decouple the configurations from the application. Then we can use a custom configuration provider to send HTTP request or query database to load those configurations. For example, in a custom configuration provider, we can load configuration data from a master database, which controls settings for multiple instances of application.

In this post, for simplicity purposes, we are going to implement a basic custom configuration provider that reads configurations from a SQLite database. The full solution is in this GitHub repository.

# Deploy an ASP.NET Core App to NGINX on Docker

## 1. Create SSL Certificate

[document](https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https)

### Windows using Linux containers

Generate certificate and configure local machine:

```bash
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p { password here }
dotnet dev-certs https --trust
```

```bash
docker-compose build
docker-compose scale core-app=4
docker-compose up
```

# Host an ASP.NET Core App with Nginx and Docker: SSL and Load Balancing

## Load Balancing

```bash
docker-compose build
docker-compose up --scale api=4 --build
docker-compose up
```

## SSL

### Generate an OpenSSL certificate

On Windows, if you have _Git for Windows_ installed, then you can use the `openssl` command directly. Otherwise, the official page: [OpenSSL.Wiki: Binaries](https://wiki.openssl.org/index.php/Binaries) contains useful URLs for downloading and installation guides.

```bash
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout localhost.key -out localhost.crt -passin pass:YourStrongPassword
```

This command will generate two files: `localhost.crt` and `localhost.key`.

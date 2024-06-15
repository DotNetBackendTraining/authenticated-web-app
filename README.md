# UTechLeague 2024 - Backend Project

## Development

1. Generate local certificate
   (replace Local__Certificates__Password with an actual password, equivalent to the one in `.env` file):
    - Windows:
    ```shell
   dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx"  -p $Local__Certificates__Password$
   dotnet dev-certs https --trust
   ```
    - Linux/MacOS:
   ```shell
   dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p $Local__Certificates__Password$
   dotnet dev-certs https --trust
   ```
2. Fill the variables in `example.env` and rename the file to `.env`
3. Run `docker-compose.development.yml`, you can use the following command:
   ```shell
   docker-compose -f "docker-compose.development.yml" up -d
   ```
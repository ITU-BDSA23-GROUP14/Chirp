docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=33eca922-74a0-11ee-9e21-00155d9a126b" -p 1433:1433 --name sql-server -d mcr.microsoft.com/mssql/server:2022-latest

dotnet run --project src/Chirp.Web
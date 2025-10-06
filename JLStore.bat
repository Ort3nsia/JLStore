@echo off

echo ----------Accetta certificati HTTPS----------
dotnet dev-certs https --trust

echo ----------Creazione Migrations----------
mkdir "./JLStore/Migrations
dotnet ef migrations add InitialCreate --project "./JLStore/JLStore.csproj"

echo ----------Creazione DataBase----------
mkdir "./JLStore/Data
dotnet ef database update --project "./JLStore/JLStore.csproj"

echo ----------Esecuzione Progetto----------
dotnet run --project "./JLStore/JLStore.csproj"

pause
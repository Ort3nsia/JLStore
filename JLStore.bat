dotnet dev-certs https --trust

mkdir "./JLStore/Migrations
dotnet ef migrations add InitialCreate --project "./JLStore/JLStore.csproj"

mkdir "./JLStore/Data
dotnet ef database update --project "./JLStore/JLStore.csproj"

dotnet run --project "./JLStore/JLStore.csproj"

pause
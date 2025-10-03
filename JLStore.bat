dotnet dev-certs https --trust
dotnet ef migrations add InitialCreate --project "./JLStore/JLStore.csproj"
dotnet ef database update --project "./JLStore/JLStore.csproj"
dotnet run --project "./JLStore/JLStore.csproj" 
pause
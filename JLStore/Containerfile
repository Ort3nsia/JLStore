# Utilizziamo dotnet:1-9.0-bookworm come immagine di base
FROM mcr.microsoft.com/devcontainers/dotnet:1-9.0-bookworm AS base
# Impostiamo la directory di lavoro
WORKDIR /app
# Esponiamo la porta per l'applicazione
#EXPOSE 5250
#EXPOSE 7250

# Fase di build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src    
# Copiamo i file di progetto e ripristiniamo le dipendenze
COPY JLStore/JLStore.csproj JLStore/
RUN dotnet restore JLStore/JLStore.csproj
# Copiamo il resto del codice sorgente
COPY . .

WORKDIR /src/JLStore
# Costruiamo l'applicazione in modalità Release
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JLStore.dll"]


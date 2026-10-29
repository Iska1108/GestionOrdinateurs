# =========================
# Build
# =========================

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copie des fichiers projets
COPY GestionOrdinateurs.API/GestionOrdinateurs.API.csproj ./GestionOrdinateurs.API/
COPY GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj ./GestionOrdinateurs.Shared/

# Restauration des dépendances
RUN dotnet restore ./GestionOrdinateurs.API/GestionOrdinateurs.API.csproj

# Copie du code source
COPY GestionOrdinateurs.API/ ./GestionOrdinateurs.API/
COPY GestionOrdinateurs.Shared/ ./GestionOrdinateurs.Shared/

# Publication de l'API
RUN dotnet publish ./GestionOrdinateurs.API/GestionOrdinateurs.API.csproj -c Release -o /app/out

# =========================
# Runtime
# =========================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5157

# Attribut différents a composer dans le Doccker Compose pour exposer le port de l'API
EXPOSE 5157

ENTRYPOINT ["dotnet", "GestionOrdinateurs.API.dll"]
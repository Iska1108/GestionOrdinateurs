# =========================
# Build
# =========================

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copie du fichier projet
COPY GestionOrdinateurs.Gateway/GestionOrdinateurs.Gateway.csproj ./GestionOrdinateurs.Gateway/

# Restauration des dépendances
RUN dotnet restore ./GestionOrdinateurs.Gateway/GestionOrdinateurs.Gateway.csproj

# Copie du code source
COPY GestionOrdinateurs.Gateway/ ./GestionOrdinateurs.Gateway/

# Publication
RUN dotnet publish ./GestionOrdinateurs.Gateway/GestionOrdinateurs.Gateway.csproj -c Release -o /app/out

# =========================
# Runtime
# =========================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5010

EXPOSE 5010

ENTRYPOINT ["dotnet", "GestionOrdinateurs.Gateway.dll"]
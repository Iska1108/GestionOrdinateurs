# =========================
# Build Blazor WebAssembly
# =========================

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copie du fichier projet
COPY GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj ./GestionOrdinateurs.Client/

# Restauration des dépendances
RUN dotnet restore ./GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj

# Copie du code source
COPY GestionOrdinateurs.Client/ ./GestionOrdinateurs.Client/

# Publication du client Blazor
RUN dotnet publish ./GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj -c Release -o /app/out

# =========================
# Serveur Nginx
# =========================

FROM nginx:alpine AS runtime

# Configuration Nginx fournie par le professeur
COPY GestionOrdinateurs.Client/nginx.conf /etc/nginx/conf.d/default.conf

# Copie des fichiers statiques Blazor
COPY --from=build /app/out/wwwroot /usr/share/nginx/html

EXPOSE 5000
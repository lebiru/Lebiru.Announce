# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Persist admin credentials
VOLUME ["/app/admin_credentials"]

# Copy static files for Razor Pages
COPY /app/wwwroot /app/wwwroot

# Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "app/Lebiru.Announce.csproj"
RUN dotnet publish "app/Lebiru.Announce.csproj" -c Release -o /app/publish

# Deploy
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Lebiru.Announce.dll"]

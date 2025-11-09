# Use .NET 8 SDK image to build your app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore dependencies
COPY . .
RUN dotnet restore ./GlowCart/GlowCart.csproj

# Build and publish app
RUN dotnet publish ./GlowCart/GlowCart.csproj -c Release -o /app/publish

# Use ASP.NET runtime to host app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GlowCart.dll"]

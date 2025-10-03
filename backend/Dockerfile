# Development stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS development
WORKDIR /app

# Copy csproj and restore dependencies
COPY src/ShowcaseGallery.Api/*.csproj ./src/ShowcaseGallery.Api/
RUN dotnet restore ./src/ShowcaseGallery.Api/ShowcaseGallery.Api.csproj

# Copy source code
COPY src/ ./src/

# Set working directory to the project
WORKDIR /app/src/ShowcaseGallery.Api

# Use watch mode for development
ENTRYPOINT ["dotnet", "watch", "run", "--no-restore", "--urls", "http://+:5000"]

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/ShowcaseGallery.Api/*.csproj ./ShowcaseGallery.Api/
RUN dotnet restore ./ShowcaseGallery.Api/ShowcaseGallery.Api.csproj

COPY src/ShowcaseGallery.Api/. ./ShowcaseGallery.Api/
RUN dotnet build ./ShowcaseGallery.Api/ShowcaseGallery.Api.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish ./ShowcaseGallery.Api/ShowcaseGallery.Api.csproj -c Release -o /app/publish

# Final production stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS production
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

# Create non-root user
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

EXPOSE 5000
ENTRYPOINT ["dotnet", "ShowcaseGallery.Api.dll"]
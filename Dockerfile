# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage for restoring, building, and publishing the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY EntLibBackendAPI/EntLibBackendAPI.csproj EntLibBackendAPI/
RUN dotnet restore "./EntLibBackendAPI/EntLibBackendAPI.csproj"

# Copy the entire source code
COPY . .
WORKDIR "/src/EntLibBackendAPI"

# Publish the application
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage for the runtime
FROM base AS final
WORKDIR /app

# Copy published files from the build stage
COPY --from=build /app/publish .

# Define the entry point for the container
ENTRYPOINT ["dotnet", "EntLibBackendAPI.dll"]

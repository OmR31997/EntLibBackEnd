# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY EntLibBackendAPI/EntLibBackendAPI.csproj EntLibBackendAPI/
RUN dotnet restore "EntLibBackendAPI.csproj"

# Copy the entire source code
COPY . .
WORKDIR "/src/EntLibBackendAPI"

# Publish the application
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EntLibBackendAPI.dll"]

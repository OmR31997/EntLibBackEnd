# Use official .NET SDK as a build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Environment variables for production
ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    FIREBASE_PATH=/etc/secrets/firebase-config.json \
    API_URL_PATH=/etc/secrets/urls-path.json

# Copy the compiled output from the build stage
COPY --from=build-env /app/out .

# Expose the port the app runs on
EXPOSE 80
EXPOSE 443

# Set the entrypoint
ENTRYPOINT ["dotnet", "EntLibBackendAPI.dll"]

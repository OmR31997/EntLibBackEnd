# Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY EntLibBackendAPI.csproj ./
RUN dotnet restore "./EntLibBackendAPI.csproj"

# Copy the rest of the application and build it
COPY ./ ./
RUN dotnet publish -c Release -o /out

# Use the ASP.NET runtime for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Expose the port
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "EntLibBackendAPI.dll"]

# Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the application and build it
COPY ./ ./
RUN dotnet publish -c Release -o /out

# Use the ASP.NET runtime for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "YourAppName.dll"]

# Expose the port
EXPOSE 80

# Use the official .NET SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["BlazorApp1.csproj", "./"]
RUN dotnet restore

# Copy the rest of the files
COPY . .

# Build the application
RUN dotnet build "BlazorApp1.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "BlazorApp1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy the published files from the build image
COPY --from=build /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "BlazorApp1.dll"] 
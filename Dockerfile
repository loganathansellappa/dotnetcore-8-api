# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file into the Docker image (restaurats.sln in the root)
COPY Restuarants.sln .

# Copy the project files for each project
COPY ./src/Restaurants.API/Restaurants.API.csproj ./src/Restaurants.API/
COPY ./src/Restaurants.Application/Restaurants.Application.csproj ./src/Restaurants.Application/
COPY ./src/Restaurants.Domain/Restaurants.Domain.csproj ./src/Restaurants.Domain/
COPY ./src/Restaurants.Infrastructure/Restaurants.Infrastructure.csproj ./src/Restaurants.Infrastructure/

COPY ./tests/Restaurants.Api.Tests/Restaurants.Api.Tests.csproj ./tests/Restaurants.Api.Tests/
COPY ./tests/Restaurants.Application.Tests/Restaurants.Application.Tests.csproj ./tests/Restaurants.Application.Tests/
COPY ./tests/Restuarants.Infrastructure.Tests/Restuarants.Infrastructure.Tests.csproj ./tests/Restuarants.Infrastructure.Tests/
# List the contents of /app to verify if all files are copied
RUN ls -l /app

# Restore dependencies for the entire solution
RUN dotnet restore Restuarants.sln  # Now, we're in /app and the solution file is in /app

# Copy the entire source code into the image
COPY . .

# Publish the app
WORKDIR /app/src/Restaurants.API
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose ports for the API
EXPOSE 80
EXPOSE 443

# Start the app
ENTRYPOINT ["dotnet", "Restaurants.API.dll"]

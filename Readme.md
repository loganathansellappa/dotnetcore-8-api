dotnet new webapi -n Restuarants.API --no-openapi -controllers
dotnet sln add ./Restuarants.API
dotnet ef migrations add --project Restaurants.Infrastructure/Restaurants.Infrastructure.csproj --startup-project Restaurants.API/Restaurants.API.csproj --context Restaurants.Infrastructure.Persistence.RestaurantsDbContext --configuration Debug --verbose Initial --output-dir Migrations

# Running the App with Docker

Follow these steps to run the application using Docker and `docker-compose`.

## 1. Prerequisites

Make sure you have the following installed on your machine:

- [Docker](https://www.docker.com/get-started) (includes Docker Compose)

You can verify that Docker is installed by running:

```bash
docker --version
docker-compose --version
git clone https://github.com/loganathansellappa/dotnetcore-8-api.git
cd dotnetcore-8-api
docker-compose up --build
```
## Swagger

http://localhost:8080/swagger/index.html

# Credentials:
testuser@testdevlogan.com / abcTest123!



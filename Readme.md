dotnet new webapi -n Restuarants.API --no-openapi -controllers
dotnet sln add ./Restuarants.API
dotnet ef migrations add --project Restaurants.Infrastructure/Restaurants.Infrastructure.csproj --startup-project Restaurants.API/Restaurants.API.csproj --context Restaurants.Infrastructure.Persistence.RestaurantsDbContext --configuration Debug --verbose Initial --output-dir Migrations



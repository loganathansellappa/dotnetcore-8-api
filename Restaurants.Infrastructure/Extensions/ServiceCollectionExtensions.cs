using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Console.WriteLine("Hello World!");


        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>() // Support Roles in Authorize decorator
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>() // Add claims to access token
            .AddEntityFrameworkStores<RestaurantsDbContext>();
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasNationality, 
                builder => builder.RequireClaim(AppClaimTypes.Nationality, "Indian", "Deutch"))
            .AddPolicy(PolicyNames.Atleast20, builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
    }
}
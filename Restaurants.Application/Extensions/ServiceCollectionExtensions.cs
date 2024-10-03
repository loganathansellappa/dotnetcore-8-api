using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        // Inform Automapper to search for profiles in current assembly
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}
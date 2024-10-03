using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
    {
        logger.LogInformation("Retrieving all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();
        var restaurantDtos = restaurants.Select(RestaurantDto.FromEntity);
        
        return restaurantDtos!;
    }

    public async Task<RestaurantDto?> GetRestaurantByIdAsync(int id)
    {
        logger.LogInformation($"Retrieving restaurant with id: {id}");
        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        
        return RestaurantDto.FromEntity(restaurant);
    }
    
}
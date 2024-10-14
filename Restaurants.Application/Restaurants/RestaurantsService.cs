using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger, IMapper mapper) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
    {
        logger.LogInformation("Retrieving all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();
        var restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        
        return restaurantsDtos!;
    }

    public async Task<RestaurantDto?> GetRestaurantByIdAsync(int id)
    {
        logger.LogInformation($"Retrieving restaurant with id: {id}");
        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);

        return restaurantDto;
    }
    
    public async Task<int> CreateRestaurantDto(CreateRestaurantDto createRestaurantDto)
    {
        logger.LogInformation($"Creating restaurant");

        var restaurant = mapper.Map<Restaurant>(createRestaurantDto);
        await restaurantsRepository.CreateAsync(restaurant);
        return restaurant.Id;
    }
    
}
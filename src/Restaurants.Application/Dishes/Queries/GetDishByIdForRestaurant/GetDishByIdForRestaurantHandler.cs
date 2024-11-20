using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

public class GetDishByIdForRestaurantHandler(ILogger<GetDishByIdForRestaurantHandler> logger, 
    IRestaurantsRepository restaurantsRepository, IDishesRepository dishesRepository, IMapper mapper) :   IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
{
    public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving dish: {@DishRequest}", request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId) ?? 
                         throw new NotFoundException(nameof(Restaurant), $"{request.RestaurantId}");
        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId) ?? throw new NotFoundException(nameof(Dish), $"{request.DishId}");
        return mapper.Map<DishDto>(dish);    
    }
}
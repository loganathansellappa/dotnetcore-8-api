using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.DeleteDishForRestaurant;

public class DeleteDishForRestaurantHandler(ILogger<DeleteDishForRestaurantHandler> logger,
    IRestaurantAuthorizationService restaurantAuthorizationService, IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteDishForRestaurantCommand> 

{
    public async Task Handle(DeleteDishForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleting restaurant - {request.RestaurantId}");
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), $"{request.RestaurantId}");
        
        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbiddenException(nameof(Restaurant), $"{request.RestaurantId}");

        await restaurantsRepository.DeleteAsync(restaurant);
    }
}
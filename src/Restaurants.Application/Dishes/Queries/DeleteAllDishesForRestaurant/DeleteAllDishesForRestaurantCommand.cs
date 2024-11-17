using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.DeleteAllDishesForRestaurant;

public class DeleteAllDishesForRestaurantCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
}   
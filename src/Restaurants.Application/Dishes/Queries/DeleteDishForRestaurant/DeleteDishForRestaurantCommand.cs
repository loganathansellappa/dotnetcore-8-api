using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.DeleteDishForRestaurant;

public class DeleteDishForRestaurantCommand(int restaurantId, int dishId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
    public int DishId { get; } = dishId;
}   
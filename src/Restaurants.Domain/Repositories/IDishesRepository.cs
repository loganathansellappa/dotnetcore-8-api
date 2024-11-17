using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository
{
    Task<int> Create(Dish entity);
    Task<IEnumerable<Dish>> GetDishesForRestaurant(int restaurantId);
    Task DeleteAllDishesForRestaurantAsync(IEnumerable<Dish> dishes);
}
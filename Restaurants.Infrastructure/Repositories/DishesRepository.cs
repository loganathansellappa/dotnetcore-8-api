using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext dbContext)  : IDishesRepository
{
    public async Task<int> Create(Dish entity)
    {
        dbContext.Dishes.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<IEnumerable<Dish>> GetDishesForRestaurant(int restaurantId)
    {
        return await dbContext.Dishes
            .Where(d => d.RestaurantId == restaurantId)
            .ToListAsync(); 
    }

    public async Task DeleteAllDishesForRestaurantAsync(int restaurantId)
    {
        var dishes =  await dbContext.Dishes
            .Where(d => d.RestaurantId == restaurantId)
            .ToListAsync();
        dbContext.Dishes.RemoveRange(dishes);
        await dbContext.SaveChangesAsync();
    }
}
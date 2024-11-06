using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant> GetByIdAsync(int id);
    Task<int> CreateAsync(Restaurant entity);
    Task<bool> UpdateAsync(Restaurant entity);
    Task DeleteAsync(Restaurant entity);
    
    Task<IEnumerable<Restaurant>> GetAllMatchingAsync(string searchTerm);
}
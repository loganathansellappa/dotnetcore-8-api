using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant> GetByIdAsync(int id);
    Task<int> CreateAsync(Restaurant entity);
    Task<bool> UpdateAsync(Restaurant entity);
    Task DeleteAsync(Restaurant entity);
    
    Task<(List<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(string searchTerm, int pageSize,
        int pageNumber, string? sortBy, SortDirection sortDirection);
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants
            .Include(r => r.Dishes) 
            .ToListAsync();
        return restaurants;
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);
        return restaurant;
    }
    
    public async Task<int> CreateAsync(Restaurant entity)
    {
        dbContext.Restaurants.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteAsync(Restaurant entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<(List<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(string searchTerm,
        int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
    {
       
        var baseQuery = dbContext.Restaurants
            .Where(r => searchTerm == null || r.Name.ToLower().Contains(searchTerm) ||
                        r.Description.ToLower().Contains(searchTerm));
        
        var totalCount = await baseQuery.CountAsync();
        
        if(sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category },
            };

            var selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }
        
        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();
        
        return (restaurants, totalCount);
    }

    public async Task<bool> UpdateAsync(Restaurant entity)
    {
        var existingEntity = await dbContext.Restaurants.FindAsync(entity.Id);
        if (existingEntity == null)
            return false;
        if (!string.IsNullOrWhiteSpace(entity.Name))
        {
            existingEntity.Name = entity.Name;
        }

        if (!string.IsNullOrWhiteSpace(entity.Description))
        {
            existingEntity.Description = entity.Description;
        }

        // Check for boolean values explicitly (ensure they are not null)
        if (entity.HasDelivery != existingEntity.HasDelivery)
        {
            existingEntity.HasDelivery = entity.HasDelivery;
        }
        try
        {
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return false;
        }
    }
}
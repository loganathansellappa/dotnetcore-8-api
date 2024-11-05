using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumRestaurantRequirement(int minimumRestaurant) : IAuthorizationRequirement
{
    public int MinimumRestaurant { get; } = minimumRestaurant;
}
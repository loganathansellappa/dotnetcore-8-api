using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

internal class MinimumRestaurantRequirementHandler(
    IRestaurantsRepository restaurantsRepository, IUserContext userContext) : AuthorizationHandler<MinimumRestaurantRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
             context.Fail();
        }

        var restaurants = await restaurantsRepository.GetAllAsync();
        var count = restaurants.Count(r => r.OwnerId == currentUser.Id);
        if (count < requirement.MinimumRestaurant)
        {
            context.Fail();
        }
        else
        {
            context.Succeed(requirement);
        }

        return;
    }
}
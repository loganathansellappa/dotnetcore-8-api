using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumRestaurantRequirementHandler(ILogger<MinimumRestaurantRequirementHandler> logger, 
    IRestaurantsRepository restaurantsRepository, IUserContext userContext) : AuthorizationHandler<MinimumRestaurantRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            logger.LogInformation($"Unable to get current user {currentUser.Id}");
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
            logger.LogInformation("User dont have required no of restaurants");
            context.Succeed(requirement);
        }

        return;
    }
}
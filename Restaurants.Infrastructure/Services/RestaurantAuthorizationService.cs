using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger, IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Authorizing user: {User} for resource operation: {ResourceOperation}", user?.Email, resourceOperation);
            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Authorizing Admin user: {User} for resource operation: {ResourceOperation}", user?.Email, resourceOperation);
            return true;
        }

        if (resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Delete && user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Authorizing owner user: {User} for resource operation: {ResourceOperation}", user?.Email, resourceOperation);
            return true;
        }
        return false;
    }
}
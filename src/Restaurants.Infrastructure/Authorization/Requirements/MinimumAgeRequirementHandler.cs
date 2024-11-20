using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger, IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null || currentUser.DateOfBirth == null)
        {
            logger.LogError("Unable to get current user");
             context.Fail();
             return Task.CompletedTask;
        }
        logger.LogInformation("Checking age requirement for user: {User} - Dob: {Dob} - Nationality - {Nationality}" , currentUser.Email, currentUser.DateOfBirth, currentUser.Nationality);
        
        if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogInformation("User has min age");
            context.Succeed(requirement);
        }
        else
        {
            logger.LogInformation("User does not have min age");
            context.Fail(); 
        }
        return Task.CompletedTask;
    }
}
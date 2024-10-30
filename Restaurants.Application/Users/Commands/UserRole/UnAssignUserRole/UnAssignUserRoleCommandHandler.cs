using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.UserRole.Base;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UserRole.UnAssignUserRole;

public class UnAssignUserRoleCommandHandler(
    UserManager<User> userManager, 
    RoleManager<IdentityRole> roleManager, 
    ILogger<UnAssignUserRoleCommandHandler> logger) : UserRoleBaseHandler<UnAssignUserRoleCommand>(logger, userManager, roleManager)
{
    public override async Task Handle(UnAssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserByEmailAsync(request.UserEmail);
        var role = await GetRoleAsync(request.RoleName);

        var result = await userManager.RemoveFromRoleAsync(user, request.RoleName);
        if (!result.Succeeded) 
            throw new NotFoundException(nameof(IdentityRole), request.RoleName);
    }
}
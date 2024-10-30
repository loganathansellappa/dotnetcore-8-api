using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.Commands.UserRole.Base;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UserRole.AssignUserRole;

public class AssignUserRoleCommandHandler(
    UserManager<User> userManager, 
    RoleManager<IdentityRole> roleManager, 
    ILogger<AssignUserRoleCommandHandler> logger) : UserRoleBaseHandler<AssignUserRoleCommand>(logger, userManager, roleManager)
{
    public override async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await GetUserByEmailAsync(request.UserEmail);
        var role = await GetRoleAsync(request.RoleName);
        
        var result = await userManager.AddToRoleAsync(user, request.RoleName);
        if (!result.Succeeded) 
            throw new NotFoundException(nameof(IdentityRole), request.RoleName);
    }
}
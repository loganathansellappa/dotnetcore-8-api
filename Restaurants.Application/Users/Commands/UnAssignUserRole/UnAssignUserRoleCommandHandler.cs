using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnAssignUserRole;

public class UnAssignUserRoleCommandHandler(ILogger<UnAssignUserRoleCommandHandler> logger,
    IUserContext userContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : IRequestHandler<UnAssignUserRoleCommand>
{
    public async Task Handle(UnAssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user =  await userManager.FindByEmailAsync(request.UserEmail);

        if (user is null)
            throw new NotFoundException(nameof(User), $"{request.UserEmail}");
        
        var role = await roleManager.FindByNameAsync(request.RoleName.ToUpperInvariant());

        if (role is null)
            throw new NotFoundException(nameof(IdentityRole), $"{request.RoleName}");

        var result = await userManager.RemoveFromRoleAsync(user, request.RoleName);
        if (!result.Succeeded) 
            throw new Exception("Unable to remove user from role");
    }
}
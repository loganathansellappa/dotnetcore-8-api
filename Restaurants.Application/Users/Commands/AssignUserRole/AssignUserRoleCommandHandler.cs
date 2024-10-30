using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger,
    IUserContext userContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(">>>>>>>>>>Assigning user role: {UserId}, with {@UserRequest}", userContext.GetCurrentUser()!.Id, request);
        var user =  await userManager.FindByEmailAsync(request.UserEmail);
        if (user is null)
            throw new NotFoundException(nameof(User), $"{request.UserEmail}");
        
        var role = await roleManager.FindByNameAsync(request.RoleName.ToUpperInvariant());
        var roles = await roleManager.Roles.ToListAsync();
        logger.LogInformation(">>>>>>>>>>Assigning user roles:  {@roles}", roles);

        if (role is null)
            throw new NotFoundException(nameof(IdentityRole), $"{request.RoleName}");

        var result = await userManager.AddToRoleAsync(user, request.RoleName);
        if (!result.Succeeded) 
            throw new Exception("Unable to add user to role");
    }
}
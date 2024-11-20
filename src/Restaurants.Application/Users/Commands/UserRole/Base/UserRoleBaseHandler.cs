using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UserRole.Base;

public abstract class UserRoleBaseHandler<TCommand>(ILogger<UserRoleBaseHandler<TCommand>> logger,
 UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : IRequestHandler<TCommand> where TCommand : UserRoleBaseCommand
{
    protected async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            throw new NotFoundException(nameof(User), email);

        return user;
    }

    protected async Task<IdentityRole> GetRoleAsync(string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName.ToUpperInvariant());
        if (role is null)
            throw new NotFoundException(nameof(IdentityRole), roleName);

        return role;
    }
    
    public abstract Task Handle(TCommand request, CancellationToken cancellationToken);

}
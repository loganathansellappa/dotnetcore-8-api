using MediatR;

namespace Restaurants.Application.Users.Commands.UserRole.Base;

public abstract class UserRoleBaseCommand : IRequest
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}
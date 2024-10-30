using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UpdateUser;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers;
[ApiController]
[Route("api/identity")]
[Authorize]
public class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPatch("user")]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserCommand updateUserCommand)
    {
        await mediator.Send(updateUserCommand);
        return NoContent();
    }
    [HttpPost("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand assignUserRoleCommand)
    {
        await mediator.Send(assignUserRoleCommand);
        return NoContent();
    }
}
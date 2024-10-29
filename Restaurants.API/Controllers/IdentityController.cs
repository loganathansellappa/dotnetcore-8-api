using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands;

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
}
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
/*
 * Service Pattern replaced with CQRS patter using MediatorR
 */
// public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase 
[Authorize]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
/// <summary>
/// Retrieves a list of all restaurants based on the provided query parameters.
/// </summary>
/// <param name="query">The query parameters to filter the restaurants.</param>
/// <returns>A list of <see cref="RestaurantDto"/> representing the restaurants.</returns>
/// <remarks>
/// This operation requires the user to have at least 2 restaurants in their account
/// to be authorized to access this endpoint.
/// </remarks>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        var restaurants = await mediator.Send(query);
         return Ok(restaurants);
    }
    
    [HttpGet("{id}")]
    [Authorize(Policy = PolicyNames.Atleast20)]
    public async Task<ActionResult<RestaurantDto>> GetById([FromRoute]int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        return Ok(restaurant);
        
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
       
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById([FromRoute]int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateRestaurantCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    
    [HttpPost]
    [Route("{restaurantId}/logo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadLogo([FromRoute]int id, IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var command = new UploadRestaurantLogoCommand()
        {
            Id = id,
            File = stream,
            FileName = file.FileName

        };
            
         await mediator.Send(command);
        return NoContent();
    }   
}
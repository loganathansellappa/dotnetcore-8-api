using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
/*
 * Service Pattern replaced with CQRS patter using MediatorR
 */
// public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase 
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
        if (restaurant is null) 
            return NotFound();
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
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));
        if (isDeleted) 
            return NoContent();
        return NotFound();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(UpdateRestaurantCommand command)
    {
        var isUpdated = await mediator.Send(command);
        if (isUpdated) 
            return NoContent();
        return NotFound();
    }
}
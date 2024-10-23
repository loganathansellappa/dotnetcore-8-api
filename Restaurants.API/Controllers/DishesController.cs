using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.DeleteAllDishesForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
[ApiController]
public class DishesController(IMediator mediator) : ControllerBase
{
/// <summary>
/// Creates a new dish for a specific restaurant.
/// </summary>
/// <param name="restaurantId">The ID of the restaurant for which the dish is being created.</param>
/// <param name="command">The command containing information about the dish to be created.</param>
/// <returns>An asynchronous task that represents the action result of creating the dish.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        await mediator.Send(command);
        return Created();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllForRestaurant([FromRoute] int restaurantId)
    {
        var dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));
        return Ok(dishes);
    }
    
    [HttpGet("{dishId}")]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
        return Ok(dish);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteById([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteAllDishesForRestaurantCommand(restaurantId));
        return NoContent();
    }
}
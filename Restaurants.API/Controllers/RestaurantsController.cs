using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantsService.GetAllRestaurantsAsync();
        return Ok(restaurants);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var restaurant = await restaurantsService.GetRestaurantByIdAsync(id);
        if (restaurant is null) 
            return NotFound();
        return Ok(restaurant);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantDto createRestaurantDto)
    {
        var id = await restaurantsService.CreateRestaurantDto(createRestaurantDto);
        return CreatedAtAction(nameof(GetById), new { id }, null);
       
    }
}
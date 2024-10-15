using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQuery : IRequest<RestaurantDto?>
{
    public GetRestaurantByIdQuery(int id)
    {
        Id = id;
    }
    
    public int Id { get;  }
}
using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Application.Restaurants.Dtos.Resolvers;


namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<CreateRestaurantCommand, Restaurant>()
            .ForMember(d => d.Address, opt =>
                opt.MapFrom(src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    PostalCode = src.PostalCode
                }));
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom<BlobUriResolver>())
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
            .ForMember(dest => dest.PostalCode,
                opt => opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
            .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.Dishes));

        CreateMap<UpdateRestaurantCommand, Restaurant>();
    }
}
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

[TestSubject(typeof(RestaurantsProfile))]
public class RestaurantsProfileTest
{

       private readonly IMapper _mapper;

        public RestaurantsProfileTest()
        {
            // Create a configuration to map our Profiles
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RestaurantsProfile>();  // Add your profile here
            });

            // Initialize the mapper
            _mapper = config.CreateMapper();
            
            // Ensure that the mapping configuration is valid
           // config.AssertConfigurationIsValid();
        }

        [Fact]
        public void CreateRestaurantCommand_To_Restaurant_Should_Map_Correctly()
        {
            // Arrange
            var createRestaurantCommand = new CreateRestaurantCommand
            {
                Name = "Pizza Hut",
                Description = "Italian restaurant",
                Category = "Italian",
                HasDelivery = true,
                ContactEmail = "jKt6t@example.com",
                ContactNumber = "123-456-7890",
                City = "New York",
                Street = "5th Avenue",
                PostalCode = "10001"
            };
        
            // Act
            var restaurant = _mapper.Map<Restaurant>(createRestaurantCommand);
        
            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Address.Should().NotBeNull();
            restaurant.Address.City.Should().Be("New York");
            restaurant.Address.Street.Should().Be("5th Avenue");
            restaurant.Address.PostalCode.Should().Be("10001");
        }


        [Fact]
        public void Restaurant_To_RestaurantDto_Should_Map_Correctly()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Name = "Pizza Hut",
                Description = "Italian restaurant",
                Category = "Italian",
                HasDelivery = true,
                ContactEmail = "jKt6t@example.com",
                ContactNumber = "123-456-7890",
                Id = 1,
                Address = new Address
                {
                    City = "New York",
                    Street = "5th Avenue",
                    PostalCode = "10001"
                },
            };

            // Act
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            // Assert
            restaurantDto.Should().NotBeNull();
            restaurantDto.City.Should().Be("New York");
            restaurantDto.Street.Should().Be("5th Avenue");
            restaurantDto.PostalCode.Should().Be("10001");
            restaurantDto.Dishes.Should().NotBeNull();
           
        }

        [Fact]
        public void UpdateRestaurantCommand_To_Restaurant_Should_Map_Correctly()
        {
            // Arrange
            var updateRestaurantCommand = new UpdateRestaurantCommand
            {
                Name = "Pizza Hut updated"
            };

            // Act
            var restaurant = _mapper.Map<Restaurant>(updateRestaurantCommand);

            // Assert
            // Verify the mapping logic for UpdateRestaurantCommand to Restaurant here
            // For example, if we had a Name property:
            restaurant.Name.Should().Be("Pizza Hut updated");
        }

        [Fact]
        public void CreateRestaurantCommand_To_Restaurant_Should_Handle_Address_Properly()
        {
            // Arrange
            var createRestaurantCommand = new CreateRestaurantCommand
            {
                Name = "Pizza Hut",
                Description = "Italian restaurant",
                Category = "Italian",
                HasDelivery = true,
                ContactEmail = "jKt6t@example.com",
                ContactNumber = "123-456-7890",
                City = "Los Angeles",
                Street = "Sunset Blvd",
                PostalCode = "90001",
            };

            // Act
            var restaurant = _mapper.Map<Restaurant>(createRestaurantCommand);

            // Assert
            restaurant.Address.Should().NotBeNull();
            restaurant.Address.City.Should().Be("Los Angeles");
            restaurant.Address.Street.Should().Be("Sunset Blvd");
            restaurant.Address.PostalCode.Should().Be("90001");
        }

        [Fact]
        public void Restaurant_To_RestaurantDto_Should_Handle_Null_Address()
        {
            // Arrange
            var restaurant = new Restaurant
            {
                Name = "Pizza Hut",
                Description = "Italian restaurant",
                Category = "Italian",
                HasDelivery = true,
                ContactEmail = "jKt6t@example.com",
                ContactNumber = "123-456-7890",
            };
            // Act
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            // Assert
            restaurantDto.City.Should().BeNull();
            restaurantDto.Street.Should().BeNull();
            restaurantDto.PostalCode.Should().BeNull();
            restaurantDto.Dishes.Should().NotBeNull();
            restaurantDto.Dishes.Count.Should().Be(0);}
}
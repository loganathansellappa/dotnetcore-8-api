using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandHandler))]
public class CreateRestaurantCommandHandlerTest
{
    private readonly Mock<ILogger<CreateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly CreateRestaurantCommandHandler _handler;

    public CreateRestaurantCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new CreateRestaurantCommandHandler(
            _loggerMock.Object,
            _mapperMock.Object,
            _restaurantsRepositoryMock.Object,
            _userContextMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateRestaurantAndReturnId()
    {
        // Arrange
        var command = new CreateRestaurantCommand
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Italian",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123-456-7890",
            City = "Test City",
            Street = "Test Street",
            PostalCode = "12345"
        };

        var user = new CurrentUser("123", "user@example.com", new List<string> { "Owner" }, "US", new DateOnly(1990, 1, 1));
        var restaurant = new Restaurant { Id = 1, OwnerId = user.Id };

        _userContextMock.Setup(x => x.GetCurrentUser()).Returns(user);
        _mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);
        _restaurantsRepositoryMock.Setup(r => r.CreateAsync(restaurant)).ReturnsAsync(restaurant.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(restaurant.Id);

        _userContextMock.Verify(x => x.GetCurrentUser(), Times.Once);
        _mapperMock.Verify(m => m.Map<Restaurant>(command), Times.Once);
        _restaurantsRepositoryMock.Verify(r => r.CreateAsync(restaurant), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Creating restaurant")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once
        );
    }
}

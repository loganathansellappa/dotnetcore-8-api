using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

[TestSubject(typeof(UpdateRestaurantCommandHandler))]
public class UpdateRestaurantCommandHandlerTest
{

      private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantAuthorizationService> _authServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTest()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _authServiceMock = new Mock<IRestaurantAuthorizationService>();
        _mapperMock = new Mock<IMapper>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();

        _handler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _authServiceMock.Object,
            _mapperMock.Object,
            _restaurantsRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenRestaurantDoesNotExist()
    {
        // Arrange
        var command = new UpdateRestaurantCommand { Id = 1 };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((Restaurant)null);  // Simulate restaurant not found

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _restaurantsRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _authServiceMock.Verify(a => a.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Update), Times.Never);
        _mapperMock.Verify(m => m.Map<Restaurant>(It.IsAny<UpdateRestaurantCommand>()), Times.Never);
        _restaurantsRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Restaurant>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbiddenException_WhenUserIsNotAuthorized()
    {
        // Arrange
        var command = new UpdateRestaurantCommand { Id = 1 };
        var restaurant = new Restaurant { Id = 1 };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(restaurant);
        _authServiceMock.Setup(a => a.Authorize(restaurant, ResourceOperation.Update)).Returns(false); // Simulate authorization failure

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _handler.Handle(command, CancellationToken.None));

        _restaurantsRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _authServiceMock.Verify(a => a.Authorize(restaurant, ResourceOperation.Update), Times.Once);
        _mapperMock.Verify(m => m.Map<Restaurant>(It.IsAny<UpdateRestaurantCommand>()), Times.Never);
        _restaurantsRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Restaurant>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldUpdateRestaurant_WhenUserIsAuthorized()
    {
        // Arrange
        var command = new UpdateRestaurantCommand
        {
            Id = 1,
            Name = "Updated Restaurant",
            Description = "Updated Description"
        };

        var restaurant = new Restaurant { Id = 1 };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(restaurant);
        _authServiceMock.Setup(a => a.Authorize(restaurant, ResourceOperation.Update)).Returns(true); // Simulate authorization success
        _mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _restaurantsRepositoryMock.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _authServiceMock.Verify(a => a.Authorize(restaurant, ResourceOperation.Update), Times.Once);
        _mapperMock.Verify(m => m.Map<Restaurant>(command), Times.Once);
        _restaurantsRepositoryMock.Verify(r => r.UpdateAsync(restaurant), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Updating restaurant")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once
        );
    }
}

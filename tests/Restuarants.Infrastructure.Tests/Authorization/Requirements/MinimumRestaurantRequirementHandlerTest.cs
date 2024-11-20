using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements;

namespace Restuarants.Infrastructure.Tests.Authorization.Requirements;

[TestSubject(typeof(MinimumRestaurantRequirementHandler))]
public class MinimumRestaurantRequirementHandlerTest
{
    
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly MinimumRestaurantRequirementHandler _handler;
    
    public MinimumRestaurantRequirementHandlerTest()
    {

        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new MinimumRestaurantRequirementHandler(
            _restaurantsRepositoryMock.Object,
            _userContextMock.Object
        );
    }
    
   [Fact]
    public async Task HandleRequirementAsync_ShouldFail_WhenCurrentUserIsNull()
    {
      
        // Correct mock setup for GetCurrentUser
        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns((CurrentUser?)null);

        

        var requirement = new MinimumRestaurantRequirement(1);
        var authorizationContext = new AuthorizationHandlerContext(
            new[] { requirement }, null, null
        );

        // Act
        await _handler.HandleAsync(authorizationContext);

        // Assert
        authorizationContext.HasFailed.Should().BeTrue();
    }
    
    [Fact]
    public async Task HandleRequirementAsync_ShouldFail_WhenRestaurantCountIsLessThanRequirement()
    {
        // Arrange
    
        var currentUser = new CurrentUser ("1", "test@test.com", new List<string>(), "US", new DateOnly(1990, 1, 1));
        // Correct mock setup for GetCurrentUser
        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(currentUser);
    
        _restaurantsRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Restaurant>
        {
            new Restaurant { OwnerId = "1" }
        });
    
        var handler = new MinimumRestaurantRequirementHandler(
            _restaurantsRepositoryMock.Object,
            _userContextMock.Object
        );
    
        var requirement = new MinimumRestaurantRequirement(20); // More than owned restaurants
        var authorizationContext = new AuthorizationHandlerContext(
            new[] { requirement }, null, null
        );
    
        // Act
        await _handler.HandleAsync(authorizationContext);
    
        // Assert
        authorizationContext.HasFailed.Should().BeTrue();
    }
    
    [Fact]
    public async Task HandleRequirementAsync_ShouldSucceed_WhenRestaurantCountMeetsRequirement()
    {
        // Arrange
      
    
        var currentUser = new CurrentUser ("1", "test@test.com", new List<string>(), "US", new DateOnly(1990, 1, 1));

        _userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(currentUser);
    
        _restaurantsRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Restaurant>
        {
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = currentUser.Id }
        });
    
        var handler = new MinimumRestaurantRequirementHandler(
            _restaurantsRepositoryMock.Object,
            _userContextMock.Object
        );
    
        var requirement = new MinimumRestaurantRequirement(1); // Matches owned restaurants
        var authorizationContext = new AuthorizationHandlerContext([requirement], null, null);
    
        // Act
        await handler.HandleAsync(authorizationContext);
    
        // Assert
        authorizationContext.HasSucceeded.Should().BeTrue();
    }
}
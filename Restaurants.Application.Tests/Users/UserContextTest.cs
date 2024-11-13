using System;
using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Tests.Users;

[TestSubject(typeof(UserContext))]
public class UserContextTest
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly UserContext _userContext;

    public UserContextTest()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _userContext = new UserContext(_httpContextAccessorMock.Object);
    }

    [Fact]
    public void GetCurrentUser_ThrowsInvalidOperationException_WhenHttpContextIsNull()
    {
        // Arrange
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext)null);

        // Act & Assert
        Action act = () => _userContext.GetCurrentUser();
        act.Should().Throw<InvalidOperationException>().WithMessage("Unable context is not present");
    }

    [Fact]
    public void GetCurrentUser_ReturnsNull_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity()) // Not authenticated
        };
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = _userContext.GetCurrentUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUser_ReturnsCurrentUser_WithExpectedProperties()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, UserRoles.Admin),
            new Claim(ClaimTypes.Role, UserRoles.User),
            new Claim("Nationality", "US"),
            new Claim("DateOfBirth", "1990-01-01")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(identity)
        };
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = _userContext.GetCurrentUser();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("1");
        result.Email.Should().Be("test@example.com");
        result.Roles.Should().BeEquivalentTo(new List<string> { UserRoles.Admin, UserRoles.User });
        result.Nationality.Should().Be("US");
        result.DateOfBirth.Should().Be(new DateOnly(1990, 1, 1));
    }

    [Fact]
    public void GetCurrentUser_ReturnsCurrentUser_WithNullDateOfBirth_WhenDateOfBirthClaimIsAbsent()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, UserRoles.Admin),
            new Claim("Nationality", "US")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(identity)
        };
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = _userContext.GetCurrentUser();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("1");
        result.Email.Should().Be("test@example.com");
        result.Roles.Should().BeEquivalentTo(new List<string> { UserRoles.Admin });
        result.Nationality.Should().Be("US");
        result.DateOfBirth.Should().BeNull();
    }

    [Fact]
    public void GetCurrentUser_ReturnsCurrentUser_WithNullNationality_WhenNationalityClaimIsAbsent()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, UserRoles.User),
            new Claim("DateOfBirth", "1995-05-15")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(identity)
        };
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

        // Act
        var result = _userContext.GetCurrentUser();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("1");
        result.Email.Should().Be("test@example.com");
        result.Roles.Should().BeEquivalentTo(new List<string> { UserRoles.User });
        result.Nationality.Should().BeNull();
        result.DateOfBirth.Should().Be(new DateOnly(1995, 5, 15));
    }
}
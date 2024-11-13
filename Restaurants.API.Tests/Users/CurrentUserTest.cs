using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.API.Tests.Users;

[TestSubject(typeof(CurrentUser))]
public class CurrentUserTest
{

     [Fact]
    public void IsInRole_ReturnsTrue_WhenRoleExists()
    {
        // Arrange
        var roles = new List<string> { UserRoles.Admin, UserRoles.User };
        var currentUser = new CurrentUser("1", "test@example.com", roles, "US", new DateOnly(1990, 1, 1));

        // Act
        var result = currentUser.IsInRole(UserRoles.Admin);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsInRole_ReturnsFalse_WhenRoleDoesNotExist()
    {
        // Arrange
        var roles = new List<string> { UserRoles.User };
        var currentUser = new CurrentUser("1", "test@example.com", roles, "US", new DateOnly(1990, 1, 1));

        // Act
        var result = currentUser.IsInRole(UserRoles.Admin);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsInRole_ReturnsFalse_WhenRolesListIsEmpty()
    {
        // Arrange
        var roles = new List<string>();
        var currentUser = new CurrentUser("1", "test@example.com", roles, "US", new DateOnly(1990, 1, 1));

        // Act
        var result = currentUser.IsInRole(UserRoles.Admin);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = "1";
        var email = "test@example.com";
        var roles = new List<string> { UserRoles.User };
        var nationality = "US";
        var dateOfBirth = new DateOnly(1990, 1, 1);

        // Act
        var currentUser = new CurrentUser(id, email, roles, nationality, dateOfBirth);

        // Assert
        Assert.Equal(id, currentUser.Id);
        Assert.Equal(email, currentUser.Email);
        Assert.Equal(roles, currentUser.Roles);
        Assert.Equal(nationality, currentUser.Nationality);
        Assert.Equal(dateOfBirth, currentUser.DateOfBirth);
    }

    [Fact]
    public void Constructor_AllowsNullablePropertiesToBeNull()
    {
        // Arrange
        var id = "1";
        var email = "test@example.com";
        var roles = new List<string> { UserRoles.User };

        // Act
        var currentUser = new CurrentUser(id, email, roles, null, null);

        // Assert
        Assert.Null(currentUser.Nationality);
        Assert.Null(currentUser.DateOfBirth);
    }
}
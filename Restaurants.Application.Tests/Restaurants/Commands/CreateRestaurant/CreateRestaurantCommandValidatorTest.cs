using FluentAssertions;
using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandValidator))]
public class CreateRestaurantCommandValidatorTest
{
    private readonly CreateRestaurantCommandValidator _validator;

    public CreateRestaurantCommandValidatorTest()
    {
        _validator = new CreateRestaurantCommandValidator();
    }
  
    [Fact]
    public void Validate_ShouldHaveError_WhenNameIsTooShort()
    {
        // Arrange
        var command = new CreateRestaurantCommand { Name = "AB" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name must be between 3 and 100 characters.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new CreateRestaurantCommand { Description = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description is required");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenCategoryIsInvalid()
    {
        // Arrange
        var command = new CreateRestaurantCommand { Category = "InvalidCategory" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Category)
            .WithErrorMessage("Category is invalid. Please choose from the valid categories");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenCategoryIsValid()
    {
        // Arrange
        var validCategories = new[] { "Italian", "Indian", "Mexican", "Japanese", "American" };
        foreach (var category in validCategories)
        {
            var command = new CreateRestaurantCommand { Category = category };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Category);
        }
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenEmailIsInvalid()
    {
        // Arrange
        var command = new CreateRestaurantCommand { ContactEmail = "invalid-email" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ContactEmail)
            .WithErrorMessage("Emails is invalid");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenEmailIsValid()
    {
        // Arrange
        var command = new CreateRestaurantCommand { ContactEmail = "test@example.com" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ContactEmail);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenPostalCodeIsInvalid()
    {
        // Arrange
        var command = new CreateRestaurantCommand { PostalCode = "12345" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PostalCode)
            .WithErrorMessage("Postal code is invalid, expect format XX-XXX");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenPostalCodeIsValid()
    {
        // Arrange
        var command = new CreateRestaurantCommand { PostalCode = "12-345" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PostalCode);
    }
}
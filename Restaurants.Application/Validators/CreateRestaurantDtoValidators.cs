using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Validators;

public class CreateRestaurantDtoValidators : AbstractValidator<CreateRestaurantDto>
{
    private readonly string[] validCategories = ["Italian", "Indian", "Mexican", "Japanese", "American"];
    public CreateRestaurantDtoValidators()
    {
        RuleFor(dto => dto.Name).Length(3, 100);
        RuleFor(dto => dto.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(dto => dto.Category)
            //.Must(category => validCategories.Contains(category))
            // Shorthand for above
            .Must(validCategories.Contains)
            .WithMessage("Category is invalid. Please choose from the valid categories");
        /*    
        Custom((value, context) =>
        {
            var isValidCategory = validCategories.Contains(value);
            if (!isValidCategory)
            {
                 context.AddFailure("Category is invalid. Please choose from the valid categories");
            }
            
        });*/
        RuleFor(dto => dto.ContactEmail).EmailAddress().WithMessage("Emails is invalid");
        RuleFor(dto => dto.PostalCode).Matches(@"^\d{2}-\d{3}$").WithMessage("Postal code is invalid, expect format XX-XXX");
    }
}
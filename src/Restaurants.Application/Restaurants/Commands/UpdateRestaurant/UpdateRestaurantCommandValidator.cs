using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.HasDelivery).Must(x => x == true || x == false).WithMessage("HasDelivery must be specified.");
    }
}
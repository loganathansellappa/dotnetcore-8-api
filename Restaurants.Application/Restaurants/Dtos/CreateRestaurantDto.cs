using System.ComponentModel.DataAnnotations;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class CreateRestaurantDto
{
   // Replaced with Fluent Validation
   // [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    //[Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }
    
   // [EmailAddress(ErrorMessage = "Provide valid email address")]
    public string? ContactEmail { get; set; }
    
    //[Phone(ErrorMessage = "Provide valid Phone number")]
    public string? ContactNumber { get; set; }
    
    public string? City { get; set; }
    public string? Street { get; set; }
   // [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Provide valid postal code (XX-XXX)")]
    public string? PostalCode { get; set; }
}
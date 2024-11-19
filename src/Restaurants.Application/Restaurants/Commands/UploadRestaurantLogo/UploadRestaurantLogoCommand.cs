using MediatR;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;

public class UploadRestaurantLogoCommand : IRequest
{
    public int Id { get; set; }
    public Stream File { get; set; } = default!;
    public string FileName { get; set; } = default!;
}
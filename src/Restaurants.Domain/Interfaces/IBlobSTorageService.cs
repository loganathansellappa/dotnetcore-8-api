namespace Restaurants.Domain.Interfaces;

public interface IBlobSTorageService
{
    Task<string> UploadToBlobAsync(Stream stream, string fileName);
}
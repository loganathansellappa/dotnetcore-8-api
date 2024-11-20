namespace Restaurants.Domain.Exceptions;

public class NotFoundException(string resrouceType, string resourceIdentifier) : Exception($"{resrouceType} {resourceIdentifier} not found")
{
    
}
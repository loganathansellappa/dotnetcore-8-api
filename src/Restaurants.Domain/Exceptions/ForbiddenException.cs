namespace Restaurants.Domain.Exceptions;

public class ForbiddenException(string resrouceType, string resourceIdentifier) : Exception($"Accessing {resrouceType} forbidden for {resourceIdentifier}.")
{
    
}
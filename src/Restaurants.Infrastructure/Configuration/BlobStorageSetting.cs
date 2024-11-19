namespace Restaurants.Infrastructure.Configuration;

public class BlobStorageSetting
{
    public string ConnectionString { get; set; }
    public string ContainerName { get; set; }
}
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage;

public class BlobStorageService(IOptions<BlobStorageSetting> blobStorageSettingOptions) : IBlobSTorageService
{
    private readonly BlobStorageSetting _blobStorageSetting = blobStorageSettingOptions.Value;
    public async Task<string> UploadToBlobAsync(Stream stream, string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_blobStorageSetting.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSetting.ContainerName);
        var blocClient = containerClient.GetBlobClient(fileName);
        await blocClient.UploadAsync(stream);
        var blobUri = blocClient.Uri.ToString();
        return blobUri; 
    }
}
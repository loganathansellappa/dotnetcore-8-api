using Azure.Storage.Blobs;
using Azure.Storage.Sas;
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
    
    public string GetBlobUri(string? blobUrl)
    {
        if (blobUrl == null) return string.Empty;
        var blobServiceClient = new BlobServiceClient(_blobStorageSetting.ConnectionString);
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _blobStorageSetting.ContainerName,
            BlobName = GetBlobNameFromUrl(blobUrl),
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddDays(1)
        };
        
        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        var sasToken = sasBuilder.ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(blobServiceClient.AccountName, _blobStorageSetting.AccountKey)).ToString();
        if (string.IsNullOrWhiteSpace(sasToken))
        {
            return string.Empty;
        }
        return $"{blobUrl}?{sasToken}";
    }

    private string GetBlobNameFromUrl(string url)
    {
        var uri = new Uri(url);
        return uri.Segments.Last();
    }
}
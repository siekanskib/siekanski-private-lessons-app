using siekanski_private_lessons_backend.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

public class BlobService
{
    private readonly AzureStorageConfig _config;

    public BlobService(AzureStorageConfig config)
    {
        _config = config;
    }

    public async Task UploadFileToStorage(Stream fileStream, string fileName, string contentType)
{
    var blobServiceClient = new BlobServiceClient(_config.ConnectionString);
    var blobContainerClient = blobServiceClient.GetBlobContainerClient(_config.ContainerName);
    var blobClient = blobContainerClient.GetBlobClient(fileName);

    var blobHttpHeaders = new BlobHttpHeaders
    {
        ContentType = contentType
    };

    await blobClient.UploadAsync(fileStream, new BlobUploadOptions
    {
        HttpHeaders = blobHttpHeaders
    });
}

    public string GenerateSasUrl(string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_config.ConnectionString);
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(_config.ContainerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        return $"{blobClient.Uri}{_config.SasToken}";
    }
}

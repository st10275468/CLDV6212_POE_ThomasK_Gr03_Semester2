using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageService(IConfiguration configuration)
        {

            if (configuration == null) { 

            throw new ArgumentNullException(nameof(configuration));
        }
            _blobServiceClient = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        public async Task UploadBlobAsync(string sName, string fName, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(sName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(fName);
            await blobClient.UploadAsync(content, true);
        }

    }
}

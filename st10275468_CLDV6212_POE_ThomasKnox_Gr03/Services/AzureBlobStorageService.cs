using Azure.Storage.Blobs;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{

    public class AzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        // Initializing the blob service using the connection string from azure
        public AzureBlobStorageService(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _blobServiceClient = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        // Method to upload a file to Azure Blob Storage
        public async Task<bool> UploadBlobAsync(string containerName, string fileName, Stream content)
        {
            try
            {
                // Get reference to the container
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                // Create the container if it doesn't exist
                await containerClient.CreateIfNotExistsAsync();

                // Get reference to the blob in the container (upload to specific folder or container)
                var blobClient = containerClient.GetBlobClient(fileName);

                // Upload the file to the blob storage
                await blobClient.UploadAsync(content, true); // 'true' allows overwrite

                // Return true if upload was successful
                return true;
            }
            catch (Exception ex)
            {
                // Optionally log the exception here (ex.Message)
                return false; // Return false if any error occurs during upload
            }
        }
    }







    /*public class AzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        //Initializing the blob service using the connection string from azure
        public AzureBlobStorageService(IConfiguration configuration)
        {
            if (configuration == null)
            {

                throw new ArgumentNullException(nameof(configuration));
            }

            _blobServiceClient = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        //Method created to upload product images to the azure blob service storage
        public async Task UploadBlobAsync(string sName, string fName, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(sName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(fName);
            await blobClient.UploadAsync(content, true);
        }

    }*/
}

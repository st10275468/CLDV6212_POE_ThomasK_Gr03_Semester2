using Azure.Storage.Blobs;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{

    public class AzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly HttpClient _httpClient;

        // Initializing the blob service using the connection string from azure
        public AzureBlobStorageService(IConfiguration configuration, HttpClient httpClient)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _blobServiceClient = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
            _httpClient = httpClient;
        }

        // Method to upload a file to Azure Blob Storage
        public async Task<bool> UploadBlobAsync(string containerName, string blobName, Stream content)
        {
            try
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StreamContent(content), "file", blobName) ;

                var requestUrl = $"https://cldvfunctions.azurewebsites.net/api/writeBlobFunction?code=4EfNMiYnSQe6neQrhnErbYkZv5tTv4a67gcoloz7sEv7AzFu2AAkVQ%3D%3D&conName={containerName}&blobName={blobName}";

                var response = await _httpClient.PostAsync(requestUrl, formData);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Optionally log the exception here (ex.Message)
                return false; // Return false if any error occurs during upload
            }
        }
    }



}

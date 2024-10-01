using Azure.Storage.Blobs;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{

    public class AzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly HttpClient _httpClient;

        
        public AzureBlobStorageService(IConfiguration configuration, HttpClient httpClient)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _blobServiceClient = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
            _httpClient = httpClient;
        }

        // Method created that uses the function to upload media to the storage
        public async Task<bool> UploadBlobAsync(string containerName, string blobName, Stream content)
        {
            try
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StreamContent(content), "file", blobName) ;
                //Creating the request url
                var requestUrl = $"https://cldvfunctions.azurewebsites.net/api/writeBlobFunction?code=4EfNMiYnSQe6neQrhnErbYkZv5tTv4a67gcoloz7sEv7AzFu2AAkVQ%3D%3D&conName={containerName}&blobName={blobName}";
                //Sending the request to the function
                var response = await _httpClient.PostAsync(requestUrl, formData);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
               
                return false; 
            }
        }
    }



}
/*//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/ [Accessed: 1 October 2024].
*/
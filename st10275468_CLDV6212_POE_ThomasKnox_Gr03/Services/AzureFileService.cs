
using Azure.Storage.Files.Shares;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureFileService
    {
        private readonly ShareServiceClient _shareServiceClient;
        private readonly HttpClient _httpClient;

       public AzureFileService(IConfiguration configuration, HttpClient httpClient)
        {
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
            _httpClient = httpClient ;   
        }

        public async Task UploadFileAsync(string shareName, string fileName, Stream content)
        {

            var requestUrl = $"https://cldvfunctions.azurewebsites.net/api/writeFileFunction?code=4EfNMiYnSQe6neQrhnErbYkZv5tTv4a67gcoloz7sEv7AzFu2AAkVQ%3D%3D&shareName={shareName}&fileName={fileName}"; 
           
            using var formContent = new MultipartFormDataContent();

            var streamContent = new StreamContent(content);

            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            formContent.Add(streamContent, "file", fileName);
            try
            {
                var response = await _httpClient.PostAsync(requestUrl, formContent);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync(); // Read as string for more detailed error info
                    throw new HttpRequestException($"Error uploading file: {response.StatusCode} - {responseContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Log or handle the specific HTTP request exception
                throw new Exception("An error occurred while uploading the file.", ex);
            }



           
        }
    }
}

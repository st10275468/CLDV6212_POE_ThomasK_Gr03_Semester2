
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
        private readonly string _functionURl = "https://cldvfunctions.azurewebsites.net/api/writeFileFunction?";
        public AzureFileService(IConfiguration configuration, HttpClient httpClient)
        {
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));   
        }

        public async Task UploadFileToShareAsync(string fileName, string shareName, Stream content)
        {
            var url = $"{_functionURl}shareName={Uri.EscapeDataString(shareName)}&fileName={Uri.EscapeDataString(fileName)}";
            using var formContent = new MultipartFormDataContent();
            var streamContent = new StreamContent(content);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            formContent.Add(streamContent, "file-storage", fileName);
            try
            {
                var response = await _httpClient.PostAsync(url, formContent);

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



            /* if (file == null || file.Length <= 0)
                 return false;

             var shareClient = _shareServiceClient.GetShareClient(shareName);
             await shareClient.CreateIfNotExistsAsync();

             var directoryClient = shareClient.GetDirectoryClient(directoryName);
             await directoryClient.CreateIfNotExistsAsync();

             var fileClient = directoryClient.GetFileClient(file.FileName);
             await fileClient.CreateAsync(file.Length);

             using (var stream = file.OpenReadStream())
             {
                 await fileClient.UploadAsync(stream);
             }

             return true;
         }*/
        }
    }
}

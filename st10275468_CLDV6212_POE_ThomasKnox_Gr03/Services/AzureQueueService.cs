using Azure.Storage.Files.Shares;
using Azure.Storage.Queues;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureQueueService
    {
        private readonly HttpClient _httpClient;
        private readonly ShareServiceClient _shareServiceClient;
        public AzureQueueService(IConfiguration configuration, HttpClient httpClient)
        {
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
            _httpClient = httpClient;
        }

        public async Task UploadMessageAsync(string queueName, string queueMessage)
        {
            var requestUrl = $"https://cldvfunctions.azurewebsites.net/api/transactionQueueFunction?code=4EfNMiYnSQe6neQrhnErbYkZv5tTv4a67gcoloz7sEv7AzFu2AAkVQ%3D%3D&queueName={queueName}&queueMessage={queueMessage}";

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            var response = await _httpClient.SendAsync(request);
        }
    }
}

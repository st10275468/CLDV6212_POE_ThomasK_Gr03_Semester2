using Azure.Storage.Queues;
using System.Text;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureQueueService
    {
        private readonly HttpClient _httpClient;
        private readonly string functionUrl = "https://cldvfunctions.azurewebsites.net/api/transactionQueueFunction";
        public AzureQueueService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendMessageAsync(string queueName, string message)
        {
            var url = $"{functionUrl}?queueName={Uri.EscapeDataString(queueName)}&message={Uri.EscapeDataString(message)}";

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var response = await _httpClient.SendAsync(request);
        }
    }
}

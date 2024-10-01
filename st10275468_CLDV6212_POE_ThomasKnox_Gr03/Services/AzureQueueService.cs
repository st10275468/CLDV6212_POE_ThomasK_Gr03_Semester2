using Azure.Storage.Queues;
using System.Text;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureQueueService
    {
        private readonly QueueServiceClient _queueServiceClient;

        public AzureQueueService(IConfiguration configuration)
        {
            _queueServiceClient = new QueueServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        public async Task SendMessageAsync(string queueName, string message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            // Create the queue if it doesn't already exist
            await queueClient.CreateIfNotExistsAsync();
            // Encode message to Base64 to ensure it is correctly processed
            string base64Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
            await queueClient.SendMessageAsync(base64Message);
        }
    }
}

using Azure.Storage.Queues;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureQueueService
    {
        private readonly QueueServiceClient _azureQueueServiceClient;

        public AzureQueueService(IConfiguration configuration)
        {
            _azureQueueServiceClient = new QueueServiceClient(configuration["AzureStorage:ConnectionString"]);

        }

        public async Task SendMessageAsync(string qName, string message)
        {
            var queueClient = _azureQueueServiceClient.GetQueueClient(qName);
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(message);
        }

    }
}

﻿using Azure.Storage.Queues;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureQueueService
    {
        private readonly QueueServiceClient _azureQueueServiceClient;


        public AzureQueueService(IConfiguration configuration)
        {
            _azureQueueServiceClient = new QueueServiceClient(configuration["AzureStorage:ConnectionString"]);
            //Initializing the Queue service using the connection string from azure

        }

        //Method created to send a message to the azure queue service. The message is a parameter so will be different each time you call the method
        public async Task SendMessageAsync(string qName, string message)
        {
            var queueClient = _azureQueueServiceClient.GetQueueClient(qName);
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(message);
        }

    }
}

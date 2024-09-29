using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace st10275468_CLDV6212_PoePart2_Sem2_Functions
{
    public class transactionQueueFunction
    {
        private readonly ILogger<transactionQueueFunction> _logger;

        public transactionQueueFunction(ILogger<transactionQueueFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(transactionQueueFunction))]
        public void Run([QueueTrigger("processing-queue", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}

using System;
using System.Text;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;

namespace st10275468_CLDV6212_PoePart2_Sem2_Functions.Functions
{
    public class transactionQueueFunction
    {
        private readonly AzureQueueService _azureQueueService;
        private readonly ILogger<transactionQueueFunction> _logger;

        public transactionQueueFunction(AzureQueueService azureQueueService, ILogger<transactionQueueFunction> logger)
        {
            _azureQueueService = azureQueueService;
            _logger = logger;
        }

        [Function("transactionQueueFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("Processing an order request.");

            string queueName = req.Query["queueName"];
            string queueMessage = req.Query["message"];

            var connectionString = Environment.GetEnvironmentVariable("connectionStorage");

            var queueServiceClient = new QueueServiceClient(connectionString);

            var queueClient = queueServiceClient.GetQueueClient(queueName);

            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(queueMessage);
            
            return new OkObjectResult(true);
        }
    }

    }


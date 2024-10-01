using System;
using System.Text;
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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("Processing an order request.");

            string orderID = req.Form["orderID"];
            if (string.IsNullOrWhiteSpace(orderID))
            {
                return new BadRequestObjectResult("Order ID cannot be empty.");
            }

            try
            {
                // Queue a message with the order information
                await _azureQueueService.SendMessageAsync("processing-queue", $"Processing order {orderID}");
                _logger.LogInformation($"Order {orderID} has been sent to the processing queue.");
                return new OkObjectResult($"Order {orderID} has been sent to the processing queue.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send order {orderID} to the processing queue: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }

    }


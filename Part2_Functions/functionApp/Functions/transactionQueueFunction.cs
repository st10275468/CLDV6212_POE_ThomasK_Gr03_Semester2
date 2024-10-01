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
       
        [Function("transactionQueueFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request)
        {
            

            string queueName = request.Query["queueName"];
            string queueMessage = request.Query["queueMessage"];

            if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(queueMessage))
            {
                return new BadRequestObjectResult("Must provide queue name and message");
            }
            try
            {
                var conString = Environment.GetEnvironmentVariable("connectionStorage");

                var queueServiceClient = new QueueServiceClient(conString);

                var queueClient = queueServiceClient.GetQueueClient(queueName);

                await queueClient.CreateIfNotExistsAsync();
                await queueClient.SendMessageAsync(queueMessage);

                return new OkObjectResult(true);
            }
            catch  {

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
    }

    }


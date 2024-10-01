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
       //Function that runs when triggered
        [Function("transactionQueueFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request)
        {
            
            //Request parameters
            string queueName = request.Query["queueName"];
            string queueMessage = request.Query["queueMessage"];

            // If the above variables are null, it will prompt user to provide inputs

            if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(queueMessage))
            {
                return new BadRequestObjectResult("Must provide queue name and message");
            }
            try
            {
                //Getting the connection string
                var conString = Environment.GetEnvironmentVariable("connectionStorage");
               //Connecting to the queue service
                var queueServiceClient = new QueueServiceClient(conString);
                //Getting the specific queue service with that queue name
                var queueClient = queueServiceClient.GetQueueClient(queueName);

                await queueClient.CreateIfNotExistsAsync();
                await queueClient.SendMessageAsync(queueMessage);
                //Sending the message the queue

                return new OkObjectResult(true);
            }
            catch  {

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
    }

    }
/*//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/ [Accessed: 1 October 2024].
Call, B. M. (2024, September). CLDV_FunctionsApp. Retrieved from Git Hub: https://github.com/ByronMcCallLecturer/CLDV_FunctionsApp/tree/master */

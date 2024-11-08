    using Azure.Data.Tables;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
    using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    namespace st10275468_CLDV6212_PoePart2_Sem2_Functions.Functions
    {
        public class StoreTableFunction
        {
        
            [Function("StoreTableFunction")]
            public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request)
            {
                //Retrieving the parameters
                string tblName = request.Query["tableName"];
                string partitionKey = request.Query["partitionKey"];
                string rowKey = request.Query["rowKey"];
                string name = request.Query["name"];
                string surname = request.Query["surname"];
                string email = request.Query["email"];
                string number = request.Query["number"];

                //Checking if the paramaters have been inputted
                if (string.IsNullOrEmpty(tblName) ||
                    string.IsNullOrEmpty(partitionKey) ||
                    string.IsNullOrEmpty(rowKey) ||
                    string.IsNullOrEmpty(name) ||
                    string.IsNullOrEmpty(surname) ||
                    string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(number))
                {
                    return new BadRequestObjectResult("All fields must be provided");
                }
                try
                {
                    //Getting the connection string
                    var conString = Environment.GetEnvironmentVariable("connectionStorage");
                    var serviceClient = new TableServiceClient(conString);
                    var tableClient = serviceClient.GetTableClient(tblName);
                    await tableClient.CreateIfNotExistsAsync();
                    //Creating a new table entity with the parameters
                    var entity = new TableEntity(partitionKey, rowKey) { ["name"] = name, ["surname"] = surname, ["email"] = email, ["number"] = number };

                    await tableClient.AddEntityAsync(entity);
                    //Adding the entity to the table 
                    return new OkObjectResult("Customer added to table");
                }
                catch
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
               
            }
        }


    }

    /*//Reference List:
    //OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/ [Accessed: 1 October 2024].
    Call, B. M. (2024, September). CLDV_FunctionsApp. Retrieved from Git Hub: https://github.com/ByronMcCallLecturer/CLDV_FunctionsApp/tree/master */
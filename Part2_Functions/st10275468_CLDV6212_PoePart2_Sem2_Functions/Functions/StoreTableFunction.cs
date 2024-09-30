using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;

namespace st10275468_CLDV6212_PoePart2_Sem2_Functions.Functions
{
    public class StoreTableFunction
    {
        private readonly AzureTableStorageService _tableStorageService;

        // Dependency injection for the storage service
        public StoreTableFunction(AzureTableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;

        }
        [Function("StoreTableInfo")]
        public async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            log.LogInformation("Processing a request to store customer details.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"Request body: {requestBody}"); // Log the incoming request body

            var customer = JsonConvert.DeserializeObject<CustomerDetails>(requestBody);

            // Check if deserialization succeeded
            if (customer == null)
            {
                Console.WriteLine(("CustomerDetails is null. Request body: " + requestBody));
                return new BadRequestObjectResult("Invalid customer details. Please provide valid data.");
            }

            try
            {
                // Check if the service is initialized and working
                await _tableStorageService.AddEntityAsync(customer);
                Console.WriteLine("Customer added successfully.");
                return new OkResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
                return new StatusCodeResult(500); // Internal Server Error
            }
        }
       
    }
    
    
}


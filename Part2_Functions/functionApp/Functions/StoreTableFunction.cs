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
        private readonly ILogger<StoreTableFunction> _logger;
        private readonly AzureTableStorageService _azureTableStorageService;

        public StoreTableFunction(ILogger<StoreTableFunction> logger, AzureTableStorageService azureTableStorageService)
        {
            _logger = logger;
            _azureTableStorageService = azureTableStorageService;
        }

        
        [Function("StoreTableFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to add a customer to Azure Table Storage.");

            var customer = await req.ReadFromJsonAsync<CustomerDetails>();
            if (customer == null)
            {
                _logger.LogWarning("No customer details provided.");
                return new BadRequestObjectResult("No customer details provided.");
            }

            try
            {
                await _azureTableStorageService.AddEntityAsync(customer);
                _logger.LogInformation($"Customer {customer.name} added successfully.");
                return new OkObjectResult($"Customer {customer.name} added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding customer: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }


}


using Microsoft.AspNetCore.Mvc;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Controllers
{
    public class DataManagementController : Controller
    {
        private readonly AzureTableStorageService _azureTableStorageService;
        private readonly AzureQueueService _azureQueueService;
        public DataManagementController(AzureTableStorageService azureTableStorageService, AzureQueueService azureQueueService)
        {
            _azureTableStorageService = azureTableStorageService;
            _azureQueueService = azureQueueService;
        }


        [HttpPost]
        public async Task<IActionResult> AddCustomerDetails(CustomerDetails customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _azureTableStorageService.AddEntityAsync(customer);
                    TempData["SuccessMessage"] = "Customer details added";
                    await _azureQueueService.SendMessageAsync("processing-queue", $"Creating customer profile: {customer.name}");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occured";

                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid data. Please check the input and try again.";
            }
            return View("DataManagement", customer);
        }
        [HttpGet]
        public IActionResult DataManagement()
        {
            return View();
        }

    }
}

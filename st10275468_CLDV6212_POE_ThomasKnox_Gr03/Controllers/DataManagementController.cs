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
                    await _azureQueueService.SendMessageAsync("processing-queue", $"Creating customer profile: {customer.name}");
                    return RedirectToAction("DataManagement");
                }
                catch
                {
                    return RedirectToAction("DataManagement");
                }
            }
            return RedirectToAction("DataManagement");
        }


        [HttpGet]
        public async Task<IActionResult> DataManagement()
        {
            try
            {
                var customerProfiles = await _azureTableStorageService.GetAllEntitiesAsync();
                return View(customerProfiles); 
            }
            catch
            {
                
                return View(new List<CustomerDetails>()); 
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomerProfiles()
        {
            try
            {
                var customerProfiles = await _azureTableStorageService.GetAllEntitiesAsync();
                return PartialView("CustomerProfilesPartial", customerProfiles);
            }
            catch
            {
               
                return PartialView("CustomerProfilesPartial", new List<CustomerDetails>()); 
            }
        }



    }
}

using Microsoft.AspNetCore.Mvc;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Controllers
{
    public class DataManagementController : Controller
    {
        private readonly AzureTableStorageService _azureTableStorageService;

        public DataManagementController(AzureTableStorageService azureTableStorageService)
        {
            _azureTableStorageService = azureTableStorageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerDetails(CustomerDetails customer)
        {
            if (ModelState.IsValid)
            {
                await _azureTableStorageService.AddEntityAsync(customer);

            }
            return RedirectToAction("Index");
        }
    }
}

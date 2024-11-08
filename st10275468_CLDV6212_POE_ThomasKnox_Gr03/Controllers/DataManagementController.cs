using Microsoft.AspNetCore.Mvc;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
using System.Net.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Controllers
{
    public class DataManagementController : Controller
    {
        private readonly AzureTableStorageService _azureTableStorageService;
        private readonly AzureQueueService _azureQueueService;
        private readonly HttpClient _httpClient;
        public DataManagementController(HttpClient httpClient, AzureTableStorageService azureTableStorageService, AzureQueueService azureQueueService)
        {
            //Constructor created to initialize the 2 services that im using
            _azureTableStorageService = azureTableStorageService;
            _azureQueueService = azureQueueService;
            _httpClient = httpClient;
        }

        //Method created that gets the input and passes it to the storage service through a method
        [HttpPost]
        public async Task<IActionResult> AddCustomerDetails(CustomerDetails customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Using a method to transfer the data to the storage service
                    var message = await _azureTableStorageService.AddCustomerAsync(customer);
                    TempData["SuccessMessage"] = message;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            await _azureTableStorageService.InsertCustomerProfile(customer);



            // Always fetch and return the updated customer profiles regardless of success or failure
            var customerProfiles = await _azureTableStorageService.GetAllEntitiesAsync();
            return View("DataManagement", customerProfiles);
        }

        //Method created to get all the customer data from the azure table service
        //and display it on the Datamanagement view
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
                //Will display an empty list if an error were to occur
            }
        }

        //Method created to that gets all the customer data from the azure table storage
        //used to refresh the data so that it can have the most recent customer data
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
/*//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/ [Accessed: 1 October 2024].
*/
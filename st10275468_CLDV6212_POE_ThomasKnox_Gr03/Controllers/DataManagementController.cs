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
            //Constructor created to initialize the 2 services that im using
            _azureTableStorageService = azureTableStorageService;
            _azureQueueService = azureQueueService;
        }

        //Method created to add a new customer profie to the table storage service on azure
        [HttpPost]
        public async Task<IActionResult> AddCustomerDetails(CustomerDetails customer)
        {
            //Making sure the data is valid
            if (ModelState.IsValid)
            {
                try
                {   //Saving the valid information into the table storage on azure
                    await _azureTableStorageService.AddEntityAsync(customer);
                    //Created a queue that sends a message to the queue service stating that
                    //a new customer was created
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
//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/[Accessed: 18 August 2024].

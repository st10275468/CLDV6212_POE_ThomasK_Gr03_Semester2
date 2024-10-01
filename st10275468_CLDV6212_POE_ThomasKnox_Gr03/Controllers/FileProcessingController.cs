using System.Text;
using Microsoft.AspNetCore.Mvc;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
using System.Net.Http.Headers;
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Controllers

{
    [Route("[controller]")]
    public class FileProcessingController : Controller
    {
        //Injecting azure service dependancies
        private readonly AzureFileService _azureFileService;
        private readonly AzureQueueService _azureQueueService;
        private readonly AzureBlobStorageService _azureBlobStorageService;

        private readonly HttpClient _httpClient;
        public FileProcessingController( AzureFileService azureFileService, AzureQueueService azureQueueService, AzureBlobStorageService azureBlobService, HttpClient httpClient)
        {
            //initializing the services
            _azureFileService = azureFileService;
            _azureQueueService = azureQueueService;
            _azureBlobStorageService = azureBlobService;
            _httpClient = httpClient;

        }


    [HttpGet] 
    public IActionResult FileProcessing()
      {
          return View("FileProcessing");
      }


        //Calls the service method and passes the data to it
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null) 
            {
                try
                {
                    using var stream = file.OpenReadStream();
                    //Calling the method in the fileservice class and passing the data to it
                    await _azureFileService.UploadFileAsync("file-storage", file.FileName , stream);
                    ViewBag.Message = "File uploaded.";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Error uploading file: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Message = "No file uploaded.";
            }

            return View("FileProcessing");
        }

  [HttpPost("UploadMedia")] 
        public async Task<IActionResult> UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "No file uploaded.";
                return View("FileProcessing");
            }

            var conName = "multimedia-blob-storage";
            var blobName = file.FileName;
            //Calling the method in the blobservice class and passing the data to it
            if (await _azureBlobStorageService.UploadBlobAsync(conName, blobName, file.OpenReadStream()))
            {
                ViewBag.Message = $"Media file {file.FileName} uploaded";
            }
            else
            {
                ViewBag.Message = "Media upload failed.";
            }

            return View("FileProcessing");


        }

        [HttpPost("ProcessOrder")] 
        public async Task<IActionResult> ProcessOrder(string orderID)
        {
            if (!string.IsNullOrWhiteSpace(orderID))
            {
                try
                {
                    //Calling the method in the queueservice class and passing the data to it
                    await _azureQueueService.UploadMessageAsync("processing-queue", $"Processing order {orderID}");
                    ViewBag.Message = $"Order {orderID} is being processed";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Failed to process Order {orderID} : {ex.Message}";
                }
            }
            else
            {
                ViewBag.Message = "Order ID must have a value";
            }

           
            return await Task.FromResult(RedirectToAction("FileProcessing"));
        }



    }
}
/*//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/ [Accessed: 1 October 2024].
*/


using System.Text;
using Microsoft.AspNetCore.Mvc;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
using System.Net.Http.Headers;
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Controllers

{
    [Route("[controller]")]
    public class FileProcessingController : Controller
    {
        private readonly AzureFileService _azureFileService;
        private readonly AzureQueueService _azureQueueService;
        private readonly AzureBlobStorageService _azureBlobStorageService;

        private readonly HttpClient _httpClient;
        public FileProcessingController( AzureFileService azureFileService, AzureQueueService azureQueueService, AzureBlobStorageService azureBlobService, HttpClient httpClient)
        {
            _azureFileService = azureFileService;
            _azureQueueService = azureQueueService;
            _azureBlobStorageService = azureBlobService;
            _httpClient = httpClient;

        }

        [HttpGet] // Route for the initial FileProcessing view
        public IActionResult FileProcessing()
        {
            return View("FileProcessing");
        }



        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    using var stream = file.OpenReadStream();
                    await _azureFileService.UploadFileAsync("file-storage", file.FileName , stream);
                    ViewBag.Message = "File uploaded successfully.";
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

  [HttpPost("UploadMedia")] // Specify the route for media uploads
        public async Task<IActionResult> UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "No file uploaded.";
                return View("FileProcessing");
            }

            var conName = "multimedia-blob-storage";
            var blobName = file.FileName;

            if (await _azureBlobStorageService.UploadBlobAsync(conName, blobName, file.OpenReadStream()))
            {
                ViewBag.Message = $"Media file {file.FileName} uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "Media upload failed.";
            }

            return View("FileProcessing");


        }


        [HttpPost("ProcessOrder")] // Specify the route for processing orders
        public async Task<IActionResult> ProcessOrder(string orderID)
        {
            if (!string.IsNullOrWhiteSpace(orderID))
            {
                try
                {
                    // Send the order ID to the queue
                    await _azureQueueService.SendMessageAsync("processing-queue", $"Processing order {orderID}");
                    ViewBag.Message = $"Order {orderID} has been sent to the processing queue.";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Failed to send order {orderID} to the processing queue: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Message = "Order ID cannot be empty.";
            }

            // Awaiting the RedirectToAction
            return await Task.FromResult(RedirectToAction("FileProcessing"));
        }



    }
}



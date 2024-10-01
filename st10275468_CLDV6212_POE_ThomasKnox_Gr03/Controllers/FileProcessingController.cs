using System.Text;
using Microsoft.AspNetCore.Mvc;
    using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Controllers

{
    [Route("[controller]")]
    public class FileProcessingController : Controller
        {
            private readonly AzureFileService _azureFileService;
            private readonly AzureQueueService _azureQueueService;
        private readonly AzureBlobStorageService _azureBlobStorageService;
        private readonly HttpClient _httpClient;
        public FileProcessingController(AzureFileService azureFileService, AzureQueueService azureQueueService, AzureBlobStorageService azureBlobService, HttpClient httpClient)
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

        [HttpPost("UploadFile")] // Specify the route for file uploads
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (await _azureFileService.UploadFileToShareAsync(file, "file-storage", "Files")) // Call the service method
            {
                ViewBag.Message = $"File {file.FileName} uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "File upload failed.";
            }

            return View("FileProcessing"); // Return to the view
        }

        [HttpPost("UploadMedia")] // Specify the route for media uploads
        public async Task<IActionResult> UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "No file uploaded.";
                return View("FileProcessing");
            }

            // Call the AzureBlobStorageService method to upload the media file
            if (await _azureBlobStorageService.UploadBlobAsync("multimedia-blob-storage", file.FileName, file.OpenReadStream()))
            {
                ViewBag.Message = $"Media file {file.FileName} uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "Media upload failed.";
            }

            return View("FileProcessing"); // Return to the view
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


        //Method created to allow employees to upload various multi-media to the azure blob storage service
        //Eg. Product images
        /*  [HttpPost]
          public async Task<IActionResult> UploadMedia(IFormFile file)
          {
              if (file != null)
              {
                  using var stream = file.OpenReadStream();
                  //Uploading the images to the blob service called multimedia-blob-storage using the image name
                  await _azureBlobStorageService.UploadBlobAsync("multimedia-blob-storage", file.FileName, stream);
                  //Queue created to send a message to the queue service stating that a image is being uploaded
                  await _azureQueueService.SendMessageAsync("processing-queue", $"Uploading product image: {file.FileName}");
              }
              return View("FileProcessing");
          }*/
        /* [HttpPost("ProcessOrder")]
             public async Task<IActionResult> ProcessOrder(string orderID)
             {
             await _azureQueueService.SendMessageAsync("processing-queue", $"Processing order {orderID}");
             ViewBag.Message = $"Order {orderID} has been sent to the processing queue.";
             return RedirectToAction("FileProcessing");
         }
         }*/
    }
}



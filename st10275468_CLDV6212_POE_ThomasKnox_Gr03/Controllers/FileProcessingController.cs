
    using Microsoft.AspNetCore.Mvc;
    using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
    namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Controllers
    {
        [Route("[controller]")]
        public class FileProcessingController : Controller
        {
            private readonly AzureFileService _azureFileService;
            private readonly AzureQueueService _azureQueueService;

            public FileProcessingController(AzureFileService azureFileService, AzureQueueService azureQueueService)
            {
                _azureFileService = azureFileService;
                _azureQueueService = azureQueueService;
            }

            [HttpGet] // Route for the initial FileProcessing view
            public IActionResult FileProcessing()
            {
                return View("FileProcessing");
            }

        [HttpPost("UploadFile")] // Specify the route for file uploads
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (await _azureFileService.UploadFileToShareAsync(file, "file-processing", "Directory")) // Call the service method
            {
                ViewBag.Message = $"File {file.FileName} uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "File upload failed.";
            }

            return View("FileProcessing"); // Return to the view
        }

        [HttpPost("ProcessOrder")]
            public async Task<IActionResult> ProcessOrder(string orderID)
            {
                await _azureQueueService.SendMessageAsync("processing-queue", $"Processing order {orderID}");
                return RedirectToAction("FileProcessing");
            }
        }
    }



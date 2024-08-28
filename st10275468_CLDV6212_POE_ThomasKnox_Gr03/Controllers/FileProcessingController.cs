using Microsoft.AspNetCore.Mvc;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Controllers
{
    public class FileProcessingController : Controller
    {
        private readonly AzureFileService _azureFileService;
        private readonly AzureBlobStorageService _azureBlobStorageService;
        private readonly AzureQueueService _azureQueueService;
        public FileProcessingController(AzureFileService azureFileService, AzureBlobStorageService azureBlobStorageService, AzureQueueService azureQueueService)
        {
            _azureFileService = azureFileService;
            _azureBlobStorageService = azureBlobStorageService;
            _azureQueueService = azureQueueService;
        }

        public IActionResult FileProcessing()
        {
            return View("FileProcessing");
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _azureFileService.UploadFileAsync("file-storage", file.FileName, stream);

                await _azureQueueService.SendMessageAsync("processing-queue", $"Uploading contract/document: {file.FileName}");
            }
            return View("FileProcessing");
        }

        [HttpPost]
        public async Task<IActionResult> UploadMedia(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _azureBlobStorageService.UploadBlobAsync("multimedia-blob-storage", file.FileName, stream);

                await _azureQueueService.SendMessageAsync("processing-queue", $"Uploading product image: {file.FileName}");
            }
            return View("FileProcessing");
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderID)
        {
            await _azureQueueService.SendMessageAsync("processing-queue",$"Processing order {orderID}");
            return View("FileProcessing");
        }
        
    }
}

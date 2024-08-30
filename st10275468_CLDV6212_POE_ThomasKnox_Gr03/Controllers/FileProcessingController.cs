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
            //Constructor used to initialize the azure services that we are using on this page
            _azureFileService = azureFileService;
            _azureBlobStorageService = azureBlobStorageService;
            _azureQueueService = azureQueueService;
        }

        public IActionResult FileProcessing()
        {
            return View("FileProcessing");
        }

        //Method created to allow employees to upload various files to the azure file service.
        //Eg. Contracts or documentation
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                //Uploading the file to the file service called file-storage using the files name
                await _azureFileService.UploadFileAsync("file-storage", file.FileName, stream);
                //Queue created to send a message to the queue service stating that a file is being uploaded
                await _azureQueueService.SendMessageAsync("processing-queue", $"Uploading contract/document: {file.FileName}");
            }
            return View("FileProcessing");
        }

        //Method created to allow employees to upload various multi-media to the azure blob storage service
        //Eg. Product images
        [HttpPost]
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
        }

        //Method created to that mimics processing an order as it sends a message to the queue service
        //stating that the order is being processed using the order number inputted
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderID)
        {
            //Sending the message to the queueu service called processing-queue
            await _azureQueueService.SendMessageAsync("processing-queue",$"Processing order {orderID}");
            return View("FileProcessing");
        }
        
    }
}

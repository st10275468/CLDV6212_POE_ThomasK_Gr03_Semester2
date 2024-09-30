using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;

namespace st10275468_CLDV6212_PoePart2_Sem2_Functions.Functions
{
    public class writeFileFunction
    {
        private readonly AzureFileService _azureFileService;
        private readonly AzureQueueService _azureQueueService;
        private readonly ILogger<writeFileFunction> _logger;

        public writeFileFunction(AzureFileService azureFileService, AzureQueueService azureQueueService, ILogger<writeFileFunction> logger)
        {
            _azureFileService = azureFileService;
            _azureQueueService = azureQueueService;
            _logger = logger;
        }

        [Function("writeFileFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to upload a file.");

            var file = req.Form.Files["file"];
            if (file == null || file.Length == 0)
            {
                return new BadRequestObjectResult("No file uploaded.");
            }

            // Call the AzureFileService to upload the file
            var shareName = "file-processing"; // Update with your share name
            var directoryName = "Directory"; // Update with your directory name

            bool uploadResult = await _azureFileService.UploadFileToShareAsync(file, shareName, directoryName);

            if (uploadResult)
            {
                _logger.LogInformation($"File {file.FileName} uploaded to File Storage.");
                return new OkObjectResult($"File {file.FileName} uploaded successfully.");
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }

}

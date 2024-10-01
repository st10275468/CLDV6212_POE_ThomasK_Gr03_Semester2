using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;

namespace st10275468_CLDV6212_PoePart2_Sem2_Functions.Functions
{
    public class writeBlobFunction
    {
        private readonly ILogger<writeBlobFunction> _logger;
        private readonly AzureBlobStorageService _azureBlobStorageService;

        public writeBlobFunction(ILogger<writeBlobFunction> logger, AzureBlobStorageService azureBlobStorageService)
        {
            _logger = logger;
            _azureBlobStorageService = azureBlobStorageService;
        }

        [Function("writeBlobFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to upload a media file.");

            // Retrieve the file from the HTTP request
            var file = req.Form.Files["file"];
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded.");
                return new BadRequestObjectResult("No file uploaded.");
            }

            try
            {
                // Call AzureBlobStorageService to upload the file
                using (var stream = file.OpenReadStream())
                {
                    var containerName = "multimedia-blob-storage"; // Specify the container name
                    var fileName = file.FileName;

                    bool uploadSuccess = await _azureBlobStorageService.UploadBlobAsync(containerName, fileName, stream);

                    if (uploadSuccess)
                    {
                        _logger.LogInformation($"Media file {file.FileName} uploaded successfully.");
                        return new OkObjectResult($"Media file {file.FileName} uploaded successfully.");
                    }
                    else
                    {
                        _logger.LogError($"Failed to upload media file {file.FileName}.");
                        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading media file: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

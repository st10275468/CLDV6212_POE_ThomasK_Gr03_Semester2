using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
using System;

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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request to upload a file.");


            string shareName = req.Query["shareName"];
            string fileName = req.Query["fileName"];

            if (string.IsNullOrEmpty(shareName) || (string.IsNullOrEmpty(fileName)))
            {
                return new BadRequestObjectResult("File name or sharename are not correct");

            }
            try
            {
                var conString = Environment.GetEnvironmentVariable("connectionStorage");
                var shareServiceClient = new ShareServiceClient(conString);
                var shareClient = shareServiceClient.GetShareClient(shareName);
                await shareClient.CreateIfNotExistsAsync();

                var directoryClient = shareClient.GetRootDirectoryClient();
                var fileClient = directoryClient.GetFileClient(fileName);

                if (!req.ContentType.StartsWith("multipart/form-data"))
                {
                    return new BadRequestObjectResult($"Object must be a file");
                }

                var formCollection = await req.ReadFormAsync();
                var file = formCollection.Files[0];

                await fileClient.CreateAsync(file.Length);

                using var stream = file.OpenReadStream();
                await fileClient.UploadAsync(stream);

                return new OkObjectResult("File uploaded");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            /*  var file = req.Form.Files["file"];
              if (file == null || file.Length == 0)
              {
                  return new BadRequestObjectResult("No file uploaded.");
              }

              // Call the AzureFileService to upload the file
              var shareName = "file-storage"; // Your file share name
              var directoryName = "Files"; // Your directory name

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
          */


        }
    }

}

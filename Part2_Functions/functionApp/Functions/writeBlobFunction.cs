using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;

namespace st10275468_CLDV6212_PoePart2_Sem2_Functions.Functions
{
    public class writeBlobFunction
    {

        [Function("writeBlobFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] 
            HttpRequest request, ILogger logger
            )
        {
            string conName = request.Query["conName"];
            string blobName = request.Query["blobName"];

            if (string.IsNullOrEmpty(conName) || string.IsNullOrEmpty(blobName)){

                return new BadRequestObjectResult("Must provide container and blob name");
            
            
            }

            var conString = Environment.GetEnvironmentVariable("connectionStorage");
            var blobServiceClient = new BlobServiceClient(conString);
            var containerClient = blobServiceClient.GetBlobContainerClient(conName);

            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(blobName);


            using var stream = request.Body;
            await blobClient.UploadAsync(stream, true);

            return new OkObjectResult("Multimedia uploaded");
        }
        
        
       
        
       
    }
}

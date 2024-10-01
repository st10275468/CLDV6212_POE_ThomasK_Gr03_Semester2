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
        //Function entry point when triggered
        [Function("writeBlobFunction")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest request, ILogger logger )
        {
            //Request parameters
            string conName = request.Query["conName"];
            string blobName = request.Query["blobName"];

            //Error checking, if the variables are null it will prompt the user
            if (string.IsNullOrEmpty(conName) || string.IsNullOrEmpty(blobName)){

                return new BadRequestObjectResult("Must provide container and blob name");
            
            
            }
            try
            {
                //Getting the connection string
                var conString = Environment.GetEnvironmentVariable("connectionStorage");
                var blobServiceClient = new BlobServiceClient(conString);

                var containerClient = blobServiceClient.GetBlobContainerClient(conName);
                await containerClient.CreateIfNotExistsAsync();
                //Creating the storage if it doesnt exist
                var blobClient = containerClient.GetBlobClient(blobName);

                //open the the request to read it
                using var stream = request.Body;
                //Uploading the media to the blob storage
                await blobClient.UploadAsync(stream, true);

                return new OkObjectResult("Multimedia uploaded");
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
          
       
    }
}
/*//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/ [Accessed: 1 October 2024].
Call, B. M. (2024, September). CLDV_FunctionsApp. Retrieved from Git Hub: https://github.com/ByronMcCallLecturer/CLDV_FunctionsApp/tree/master */
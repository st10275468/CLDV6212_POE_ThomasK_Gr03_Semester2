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
        

        [Function("writeFileFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request)
        {
            
            string shareName = request.Query["shareName"];
            string fileName = request.Query["fileName"];

            if (string.IsNullOrEmpty(shareName) || (string.IsNullOrEmpty(fileName)))
            {
                return new BadRequestObjectResult("Must provide share name and file name");

            }
            try
            {
                var conString = Environment.GetEnvironmentVariable("connectionStorage");
                var shareServiceClient = new ShareServiceClient(conString);
                var shareClient = shareServiceClient.GetShareClient(shareName);
               
                await shareClient.CreateIfNotExistsAsync();

                var directoryClient = shareClient.GetRootDirectoryClient();

                var fileClient = directoryClient.GetFileClient(fileName);

                if (!request.ContentType.StartsWith("multipart/form-data"))
                {
                    return new BadRequestObjectResult($"Object must be a file");
                }

                var formCollection = await request.ReadFormAsync();
                var file = formCollection.Files[0];

                await fileClient.CreateAsync(file.Length);

                using var stream = file.OpenReadStream();
                await fileClient.UploadAsync(stream);

                return new OkObjectResult("File uploaded");
            }
            catch 
            {
                
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            


        }
    }

}

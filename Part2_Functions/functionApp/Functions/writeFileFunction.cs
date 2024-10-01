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
        
        //New function created which runs when triggered
        [Function("writeFileFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request)
        {
            
            string shareName = request.Query["shareName"];
            string fileName = request.Query["fileName"];

            // If the above variables are null, it will prompt user to provide inputs
            if (string.IsNullOrEmpty(shareName) || (string.IsNullOrEmpty(fileName)))
            {
                return new BadRequestObjectResult("Must provide share name and file name");

            }
            try
            {
                //Getting the connection string
                var conString = Environment.GetEnvironmentVariable("connectionStorage");

                //Connecting to the azure file service
                var shareServiceClient = new ShareServiceClient(conString);

                //Getting the specific file service with that shareName
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
                //Opening the stream so a file can be uploaded
                using var stream = file.OpenReadStream();
                //Uploading the file
                await fileClient.UploadAsync(stream);

                return new OkObjectResult("File uploaded");
            }
            catch 
            {
                //Error checking
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            


        }
    }

}

/*//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/ [Accessed: 1 October 2024].
Call, B. M. (2024, September). CLDV_FunctionsApp. Retrieved from Git Hub: https://github.com/ByronMcCallLecturer/CLDV_FunctionsApp/tree/master */
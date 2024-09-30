
using Azure.Storage.Files.Shares;
using System.Net.Http;
using System.Net.Http.Headers;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureFileService
    {
        private readonly ShareServiceClient _shareServiceClient;

        public AzureFileService(IConfiguration configuration)
        {
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        public async Task<bool> UploadFileToShareAsync(IFormFile file, string shareName, string directoryName)
        {
            if (file == null || file.Length <= 0)
                return false;

            // Get a reference to the share
            var shareClient = _shareServiceClient.GetShareClient(shareName);

            // Create the share if it doesn't exist
            await shareClient.CreateIfNotExistsAsync();

            // Get a reference to the directory within the share
            var directoryClient = shareClient.GetDirectoryClient(directoryName);
            await directoryClient.CreateIfNotExistsAsync();

            // Get a reference to the file
            var fileClient = directoryClient.GetFileClient(file.FileName);

            await fileClient.CreateAsync(file.Length);
            // Upload the file
            using (var stream = file.OpenReadStream())
            {
                await fileClient.UploadAsync(stream); // Remove the overwrite parameter
            }

            return true;
        }
    }
}

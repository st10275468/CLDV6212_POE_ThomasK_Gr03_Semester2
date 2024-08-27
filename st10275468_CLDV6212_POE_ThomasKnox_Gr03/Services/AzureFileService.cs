
using Azure.Storage.Files.Shares;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureFileService
    {
        private readonly ShareServiceClient _shareServiceClient;

        public AzureFileService(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
        
        
        }

        public async Task UploadFileAsync(string sName, string fName, Stream content)
        {
            var shareClient = _shareServiceClient.GetShareClient(sName);
            await shareClient.CreateIfNotExistsAsync();
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fName);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);
        }
        
    
    }
}

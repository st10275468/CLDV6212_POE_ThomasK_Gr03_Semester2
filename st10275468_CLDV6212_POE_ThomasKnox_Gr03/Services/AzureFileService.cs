
using Azure.Storage.Files.Shares;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureFileService
    {
        private readonly ShareServiceClient _shareServiceClient;

        
        public AzureFileService(IConfiguration configuration)
        {
            //Throwing an exception if configuration is null
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
            //Initializing the File share service using the connection string from azure

        }

        //Method created to upload files to the azure file share service
        public async Task UploadFileAsync(string sName, string fName, Stream content)
        {
            var shareClient = _shareServiceClient.GetShareClient(sName);
            //Making sure the file share exists with that name otherwise it will create one
            await shareClient.CreateIfNotExistsAsync();
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fName);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);
        }
        
    
    }
}

using Azure.Storage.Files.Shares;
using Azure.Storage.Queues;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureQueueService
    {
        private readonly HttpClient _httpClient;
        private readonly ShareServiceClient _shareServiceClient;
        public AzureQueueService(IConfiguration configuration, HttpClient httpClient)
        {
            //Initializing the share client and http client
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
            _httpClient = httpClient;
        }

        //Method created that calls the function via the funcion URL which then processes the order and sends it to the queue
        public async Task UploadMessageAsync(string queueName, string queueMessage)
        {
            //Constructing the url
            var requestUrl = $"https://cldvfunctions.azurewebsites.net/api/transactionQueueFunction?code=4EfNMiYnSQe6neQrhnErbYkZv5tTv4a67gcoloz7sEv7AzFu2AAkVQ%3D%3D&queueName={queueName}&queueMessage={queueMessage}";
            //Making a new request
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            //Sending the request to the function
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
        
                throw new Exception($"Failed to process order. Status code: {response.StatusCode}");
            }
        }
    }
}
/*//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/ [Accessed: 1 October 2024].
*/
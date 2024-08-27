
using Azure;
using Azure.Data.Tables;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
using System.Threading.Tasks;
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureTableStorageService
    {
        private readonly TableClient _tableClient;

        public TableService(IConfiguration configuration)
        {
            var connectionString = configuration[""];
            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient("CustomerDetails");
            _tableClient.CreateIfNotExists();
        }

        public async Task AddEntityAsync(CustomerDetails customer)
        {
            await _tableClient.AddEntityAsync(customer);    
        }
    }
}


using Azure;
using Azure.Data.Tables;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
using System.Threading.Tasks;
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureTableStorageService
    {
        private readonly TableClient _tableClient;

        //Initializing the Table storage service using the connection string from azure
        public AzureTableStorageService(IConfiguration configuration)
        {
            //If the configuration is null then it will throw an exception
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            //Retrieving the connection string
            var connectionString = configuration["AzureStorage:ConnectionString"];

            //Throw a exception if the connection string is invalid
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Azure connection string is invalid");
            }
           //Creating a new instance of table service client with the valid connection string
            var serviceClient = new TableServiceClient(connectionString);

            //Getting it to interact with the table i created on azure
            _tableClient = serviceClient.GetTableClient("customerdetails");

            _tableClient.CreateIfNotExists();
        }

        //Method created to create a new customer profile and upload it as an entity on azure table service
        public async Task AddEntityAsync(CustomerDetails customer)
        {
            //Exception if customer properties are null
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            await _tableClient.AddEntityAsync(customer);
        }

        //Method created to get all the customer profiles from the table on azure
        public async Task<List<CustomerDetails>> GetAllEntitiesAsync()
        {
            try
            {
                //Getting all the customer profiles with that specific partition key
                var query = _tableClient.QueryAsync<CustomerDetails>(filter: $"PartitionKey eq 'CustomerDetails'");
                //Putting all the results in a list so that we can display it later on
                var results = new List<CustomerDetails>();
                //Using a foreach loop to go through the whole table and adding each customer to the list
                await foreach (var entity in query)
                {
                    results.Add(entity);
                }
                return results;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve customer profiles", ex);
                //Exception if an error occurs
            }
        }
    }
}
//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/[Accessed: 18 August 2024].
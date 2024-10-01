using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureTableStorageService
    {
        private readonly TableClient _tableClient;
        private readonly string _tableName = "customerdetails";
        private readonly HttpClient _httpClient;
        //Initializing the Table storage service using the connection string from azure
        public AzureTableStorageService(HttpClient httpClient ,IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var connectionString = configuration["AzureStorage:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Azure connection string is invalid");
            }

            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient(_tableName);
            _tableClient.CreateIfNotExists(); // Ensure the table exists
            _httpClient = httpClient;
        }

        public async Task<string> AddCustomerAsync(CustomerDetails customer)
        {
            try
            {
                // Constructing the request URL by appending query parameters for customer data
                var requestUrl = $"https://cldvfunctions.azurewebsites.net/api/StoreTableFunction?code=EWcNtaatLW27vpgW6GrrQQ0IraX3hSatklo7p6ogiBMfAzFuEUb2Mw%3D%3D" +
                                 $"&tableName=customerdetails" +
                                 $"&partitionKey=CustomerDetails" +
                                 $"&rowKey={customer.RowKey}" +
                                 $"&name={customer.name}" +
                                 $"&surname={customer.surname}" +
                                 $"&email={customer.email}" +
                                 $"&number={customer.number}";

                // Sending the POST request
                var response = await _httpClient.PostAsync(requestUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return $"Customer {customer.name} added successfully.";
                }
                else
                {
                    throw new Exception("Error occurred while adding customer details.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to add customer to Azure Function", ex);
            }
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
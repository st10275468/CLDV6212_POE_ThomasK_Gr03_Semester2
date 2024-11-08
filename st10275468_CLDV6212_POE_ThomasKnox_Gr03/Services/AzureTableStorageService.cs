using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services
{
    public class AzureTableStorageService
    {
        private readonly TableClient _tableClient;
        //Azure table name
        private readonly string _tableName = "customerdetails";
        private readonly HttpClient _httpClient;
        private readonly string _sqlConnectionString;
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
            //Initializing the clients
            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient(_tableName);
            _tableClient.CreateIfNotExists(); 
            //creating a new table if it doesnt exist
            _httpClient = httpClient;
            _sqlConnectionString = configuration.GetConnectionString("SqlDatabase");
            if (string.IsNullOrEmpty(_sqlConnectionString))
            {
                throw new ArgumentException("SQL connection string is invalid");
            }
        }


        //Method created that calls the function via the funcion URL which then uploads the customer details to azure
        public async Task<string> AddCustomerAsync(CustomerDetails customer)
        {
            try
            {
                //Creating the url based on the parameters
                var requestUrl = $"https://cldvfunctionsapp.azurewebsites.net/api/StoreTableFunction?code=RiljAfxvFOb_Fs5lJ-Oy_mtZLyksZVilVHV5R6sznt-AAzFulFFm4Q==" +
                                 $"&tableName=customerdetails" +
                                 $"&partitionKey=CustomerDetails" +
                                 $"&rowKey={customer.RowKey}" +
                                 $"&name={customer.name}" +
                                 $"&surname={customer.surname}" +
                                 $"&email={customer.email}" +
                                 $"&number={customer.number}";

               //Sending a request for the function
                var response = await _httpClient.PostAsync(requestUrl, null);
                
                if (response.IsSuccessStatusCode)
                {
                    
                    return $"Customer {customer.name} added";
                    //SQL statements in here


                }
                else
                {
                    throw new Exception("Error");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to upload customer", ex);
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


        public async Task InsertCustomerProfile(CustomerDetails customer)
        {
            
            var query = @"INSERT INTO Customer (name, surname, email, number)
                          VALUES (@name, @surname, @email, @number)";

            var connectionString = _sqlConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    sqlCommand.Parameters.AddWithValue("@name", customer.name);
                    sqlCommand.Parameters.AddWithValue("@surname", customer.surname);
                    sqlCommand.Parameters.AddWithValue("@email", customer.email);
                    sqlCommand.Parameters.AddWithValue("@number", customer.number);

                    connection.Open();
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and provide detailed information
                throw new InvalidOperationException("Failed to insert customer into SQL database", ex);
            }
        }
    }
}
/*//Reference List:
//OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/ [Accessed: 1 October 2024].
*/
using Azure;
using Azure.Data.Tables;
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models
{
    public class CustomerDetails : ITableEntity
    {

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        //Giving the customer certain attributes for their profile
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string number { get; set; }

        public CustomerDetails()
        {

            PartitionKey = "CustomerDetails";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}

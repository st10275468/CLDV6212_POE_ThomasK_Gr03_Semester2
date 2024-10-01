using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
using st10275468_CLDV6212_PoePart2_Sem2_Functions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace st10275468_CLDV6212_PoePart2_Sem2_Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string tableName = "customerdetails";
            builder.Services.AddSingleton<AzureTableStorageService>();

            builder.Services.AddHttpClient<AzureBlobStorageService>();
            builder.Services.AddHttpClient<AzureFileService>();
            builder.Services.AddHttpClient<AzureQueueService>();
            builder.Services.AddHttpClient<AzureTableStorageService>();

            builder.Services.AddHttpClient();
            builder.Services.AddControllersWithViews();

        }
    }
}
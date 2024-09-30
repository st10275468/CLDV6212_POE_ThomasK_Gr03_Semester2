using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(st10275468_CLDV6212_POE_ThomasKnox_Gr03.Functions.Startup))]
namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<AzureTableStorageService>();
        }
    }
}
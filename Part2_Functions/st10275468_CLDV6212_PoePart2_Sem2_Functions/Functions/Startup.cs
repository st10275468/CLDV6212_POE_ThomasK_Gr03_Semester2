using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Services;
using st10275468_CLDV6212_PoePart2_Sem2_Functions.Functions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace st10275468_CLDV6212_PoePart2_Sem2_Functions.Functions
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
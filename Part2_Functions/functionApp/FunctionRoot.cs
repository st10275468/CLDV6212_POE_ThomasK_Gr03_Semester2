using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace st10275468_CLDV6212_PoePart2_Sem2_Functions
{
    public class FunctionRoot
    {
        private readonly ILogger<FunctionRoot> _logger;

        public FunctionRoot(ILogger<FunctionRoot> logger)
        {
            _logger = logger;
        }

        [Function("HTTPTest")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}

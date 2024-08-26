using Microsoft.AspNetCore.Mvc;
using st10275468_CLDV6212_POE_ThomasKnox_Gr03.Models;
using System.Diagnostics;

namespace st10275468_CLDV6212_POE_ThomasKnox_Gr03.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DataManagement()
        {
            return View();
        }
        public IActionResult FileProcessing()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

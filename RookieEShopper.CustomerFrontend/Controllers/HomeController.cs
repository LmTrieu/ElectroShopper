using Microsoft.AspNetCore.Mvc;
using RookieEShopper.CustomerFrontend.Models;
using RookieEShopper.CustomerFrontend.Services.Category;
using RookieEShopper.CustomerFrontend.Services.Product;
using RookieEShopper.SharedLibrary.ViewModels;
using System.Diagnostics;

namespace RookieEShopper.CustomerFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryClient _categoryClient;
        private readonly IProductClient _productClient;

        private HomePageVM homePageVM = new HomePageVM();

        public HomeController(ILogger<HomeController> logger, ICategoryClient categoryClient, IProductClient productClient)
        {
            _logger = logger;
            _categoryClient = categoryClient;
            _productClient = productClient;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
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
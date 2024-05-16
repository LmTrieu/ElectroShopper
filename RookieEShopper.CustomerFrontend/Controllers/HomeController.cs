using Microsoft.AspNetCore.Mvc;
using RookieEShopper.CustomerFrontend.Models;
using RookieEShopper.CustomerFrontend.Services;
using RookieEShopper.SharedViewModel;
using System.Diagnostics;

namespace RookieEShopper.CustomerFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryClient _categoryClient;
        private readonly IProductClient _productClient;

        public HomeController(ILogger<HomeController> logger, ICategoryClient categoryClient, IProductClient productClient)
        {
            _logger = logger;
            _categoryClient = categoryClient;
            _productClient = productClient;
        }

        public async Task<IActionResult> IndexAsync()
        {
            HomePageVM navData = new HomePageVM();
            var categories = await _categoryClient.GetCategoriesAsync();
            var products = await _productClient.GetProductsAsync();

            navData.Catergories = categories.ToList();
            navData.Products = products.ToList();

            return View(navData);
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

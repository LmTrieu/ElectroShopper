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

        private HomePageVM homePageVM = new HomePageVM();


        public HomeController(ILogger<HomeController> logger, ICategoryClient categoryClient, IProductClient productClient)
        {
            _logger = logger;
            _categoryClient = categoryClient;
            _productClient = productClient;
        }

        public async Task<IActionResult> Index()
        {
            homePageVM.Catergories = (await _categoryClient.GetCategoriesAsync()).ToList();
            homePageVM.Products = (await _productClient.GetProductsAsync()).ToList();
            ViewData["NavbarVM"] = homePageVM.Catergories;

            return View(homePageVM.Products);
        }

        public async Task<IActionResult> Privacy()
        {
            homePageVM.Catergories = (await _categoryClient.GetCategoriesAsync()).ToList();
            ViewData["NavbarVM"] = homePageVM.Catergories;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

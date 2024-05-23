using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RookieEShopper.CustomerFrontend.Services;

namespace RookieEShopper.CustomerFrontend.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductClient _productClient;
        public ProductController(IProductClient productClient)
        {
            _productClient = productClient;
        }

        // GET: Shop/
        [Route("Shop")]
        public async Task<ActionResult> Index(int categoryId, string categoryName)
        {
            var products = (await _productClient.GetProductsByCategoryAsync(categoryId)).ToList();
            ViewData["CategoryName"] = categoryName;
            return View(products);
        }

        // GET: Shop/Details/5
        [Route("Shop/Details")]
        public async Task<ActionResult> Details(int productId)
        {
            var product = await _productClient.GetProductDetailById(productId);
            return View(product);
        }

    }
}

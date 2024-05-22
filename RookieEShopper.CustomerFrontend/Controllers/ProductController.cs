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

        // GET: Shop/1
        [Route("Shop")]
        public async Task<ActionResult> Index(int categoryId, string categoryName)
        {
            var products = (await _productClient.GetProductsByCategoryAsync(categoryId)).ToList();
            ViewData["CategoryName"] = categoryName;
            return View(products);
        }

        // GET: Shop/Details/5
        [Route("Shop/Details")]
        public ActionResult Details(int productId)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

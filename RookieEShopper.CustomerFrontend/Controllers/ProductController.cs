﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RookieEShopper.CustomerFrontend.Models.Dto;
using RookieEShopper.CustomerFrontend.Services.Product;
using RookieEShopper.SharedLibrary.ViewModels;

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
            ProductDetailPageVM productDetail = new ProductDetailPageVM();
            productDetail.Product = await _productClient.GetProductDetailById(productId);
            productDetail.Reviews = await _productClient.GetProductReviewsAsync(productId);

            var viewModel = (productDetail, new CreateProductReviewDto());
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [Route("CreateReview/{id}")]
        public async Task<ActionResult> CreateProductReview(int id, CreateProductReviewDto createProductReviewVM)
        {
            await _productClient.PostProductReviewAsync(createProductReviewVM);
            return RedirectToAction("Details", new { productId = createProductReviewVM.ProductId });
        }

        [Authorize]
        [Route("CreateReview/{id}")]
        public ActionResult CreateProductReview(int id)
        {
            return RedirectToAction("Details", new { productId = id });
        }
    }
}
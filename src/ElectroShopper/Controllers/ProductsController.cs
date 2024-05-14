using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NuGet.Protocol;
using RookieEShopper.Backend.Models;
using RookieEShopper.Backend.Data.Entities;
using RookieEShopper.Backend.Service.IRepositories;

namespace RookieEShopper.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<ProductRequestBodyDto> _validator;

        public ProductsController(IProductRepository productRepository, IValidator<ProductRequestBodyDto> validator)
        {
            _productRepository = productRepository;
            _validator = validator;
        }
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var result = await _productRepository.GetAllProductsAsync();
            return result is null ?
                NotFound("No product is available at the moment, try again later") : Ok(result);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            return product is null ?
                NotFound("Product not found with the specified ID.") : Ok(product);
        }

        // PUT: api/Products/5
        // Fix this shi
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductRequestBodyDto productdto)
        {
            ValidationResult validationResult = _validator.Validate(productdto);
            if (validationResult.IsValid)
            {
                return Ok(await _productRepository.IsProductExist(id) ?
                    new
                    {
                        Message = "Product updated successfully",
                        Product = await _productRepository.UpdateProductAsync(id, productdto)
                    }
                    :
                    new
                    {
                        Message = "Product created successfully",
                        Product = await _productRepository.CreateProductAsync(productdto)
                    });
            }
            else
            {
                return BadRequest(validationResult.ToDictionary().ToList());
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductRequestBodyDto productdto)
        {
            ValidationResult validationResult = _validator.Validate(productdto);
            if (validationResult.IsValid)
            {
                return Ok(await _productRepository.CreateProductAsync(productdto));
            }
            else
            {
                return BadRequest(validationResult.ToDictionary().ToList());
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product is null)
            {
                return NotFound("Product not found with the specified ID.");
            }

            return Ok(new
            {
                Message = "Product deleted successfully"
            });
        }
    }
}

using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Dto.Review;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<CreateProductDto> _productValidator;

        public ProductsController(IProductRepository productRepository, IValidator<CreateProductDto> validator)
        {
            _productRepository = productRepository;
            _productValidator = validator;
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
            var product = await _productRepository.GetDomainProductByIdAsync(id);

            return product is null ?
                NotFound("Product not found with the specified ID.") : Ok(product);
        }

        // PUT: api/Products/5
        // Fix this shi
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, CreateProductDto productdto)
        {
            ValidationResult validationResult = _productValidator.Validate(productdto);
            if (validationResult.IsValid)
            {
                return Ok(await _productRepository.IsProductExistAsync(id) ?
                    new
                    {
                        Message = "Product updated successfully",
                        Product = await _productRepository.UpdateProductAsync(id, productdto)
                    }
                    :
                    new
                    {
                        Message = "Product created successfully",
                        Product = await _productRepository.CreateProductAsync(productdto, null)
                    });
            }
            else
            {
                return BadRequest(validationResult.ToDictionary().ToList());
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(CreateProductDto productdto, [FromForm] IFormFileCollection? galleryImages)
        {
            ValidationResult validationResult = _productValidator.Validate(productdto);
            if (validationResult.IsValid)
            {
                return Ok(await _productRepository.CreateProductAsync(productdto, galleryImages));
            }
            else
            {
                return BadRequest(validationResult.ToDictionary().ToList());
            }
        }

        //[HttpPost("{id}")]
        //public async Task<ActionResult<Product>> PostProductImage(int id, IFormFile image)
        //{
        //    ValidationResult validationResult = _validator.Validate(productdto);
        //    if (validationResult.IsValid)
        //    {
        //        return Ok(await _productRepository.UploadProductImage());
        //    }
        //    else
        //    {
        //        return BadRequest(validationResult.ToDictionary().ToList());
        //    }
        //    await _productRepository.UploadProductMainImage(id, image);
        //    return Ok();
        //}
        [HttpGet]
        [Route("Category/{categoryId}")]
        public async Task<Results<Ok<IEnumerable<Product>>, NotFound<string>>> GetProductsByCategory(int categoryId)
        {
            var product = await _productRepository.GetProductsByCategoryAsync(categoryId);

            return product.Count() is 0 ?
                TypedResults.NotFound("Product not found with the specified ID.") : TypedResults.Ok(product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetDomainProductByIdAsync(id);
            if (product is null)
            {
                return NotFound("Product not found with the specified ID.");
            }
            if (await _productRepository.DeleteProductAsync(product))
                return Ok(new
                {
                    Message = "Product deleted successfully"
                });
            return BadRequest();
        }        
    }
}
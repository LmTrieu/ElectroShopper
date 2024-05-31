using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RookieEShopper.Api.Dto;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Dto.Review;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;

namespace RookieEShopper.Api.Controllers
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

        [HttpGet]
        public async Task<Results<Ok<ApiListObjectResponse<ResponseProductDto>>,NotFound<string>>> GetProducts([FromQuery] QueryParameters query)
        {
            var result = await _productRepository.GetAllProductsAsync(query);
            
            if(result.Count > 0)
            {
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

                return TypedResults.Ok(new ApiListObjectResponse<ResponseProductDto> { Data = result, Message = "Products fetched successfully", Total = result.Count() });
            }   
            return TypedResults.NotFound("No product is available at the moment, try again later");
        }

        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<Results<Ok<ApiSingleObjectResponse<ResponseDomainProductDto>>, NotFound<string>>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductDetailByIdAsync(id);

            return product is null ?
                TypedResults.NotFound("No product is available at the moment, try again later") : 
                TypedResults.Ok(new ApiSingleObjectResponse<ResponseDomainProductDto> { Data = product, Message = "Products fetched successfully"});
        }

        [Authorize]
        [EnableCors]
        [HttpPatch]
        [Route("Patch/{id}")]
        public async Task<Results<Ok<ApiSingleObjectResponse<Product>>, BadRequest<List<KeyValuePair<string, string[]>>>>> PatchProduct(int id, CreateProductDto productdto)
        {
            ValidationResult validationResult = _productValidator.Validate(productdto);
            if (validationResult.IsValid)
            {
                return TypedResults.Ok(await _productRepository.IsProductExistAsync(id) ?
                    new ApiSingleObjectResponse<Product>
                    {
                        Message = "Product updated successfully",
                        Data = await _productRepository.UpdateProductAsync(id, productdto)
                    }
                    :
                    new ApiSingleObjectResponse<Product>
                    {
                        Message = "Product created successfully",
                        Data = await _productRepository.CreateProductAsync(productdto, null)
                    });
            }
            else
            {
                return TypedResults.BadRequest(validationResult.ToDictionary().ToList());
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Patch/Stock/{id}")]
        public async Task<Results<Ok<ApiSingleObjectResponse<ResponseProductDto>>, BadRequest>> PutProductStock(int id, int numOfProduct)
        {
            if(await _productRepository.IsProductExistAsync(id))
            {
                return TypedResults.Ok(new ApiSingleObjectResponse<ResponseProductDto>
                {
                    Message = "Product updated successfully",
                    Data = await _productRepository.UpdateProductInventoryAsync(id, numOfProduct)
                });
            }
            else
            {
                return TypedResults.BadRequest();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Post")]
        public async Task<Results<Ok<ApiSingleObjectResponse<Product>>, BadRequest<List<KeyValuePair<string, string[]>>>>> PostProduct(CreateProductDto productdto, [FromForm] IFormFileCollection? galleryImages)
        {
            ValidationResult validationResult = _productValidator.Validate(productdto);
            if (validationResult.IsValid)
            {
                return TypedResults.Ok(new ApiSingleObjectResponse<Product>{
                    Message = "Product posted successfully",
                    Data = await _productRepository.CreateProductAsync(productdto, galleryImages) 
                });
            }
            else
            {
                return TypedResults.BadRequest(validationResult.ToDictionary().ToList());
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

        [Authorize]
        [HttpGet]
        [Route("Category/{id}")]
        public async Task<Results<Ok<ApiListObjectResponse<ResponseProductDto>>, NotFound<string>>> GetProductsByCategory([FromQuery] QueryParameters query, int id)
        {
            var result = await _productRepository.GetProductsByCategoryAsync(query, id);

            if (result.Count > 0)
            {
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

                return TypedResults.Ok(new ApiListObjectResponse<ResponseProductDto> { Data = result, Message = "Products fetched successfully", Total = result.Count() });
            }
            return
                TypedResults.NotFound("Product not found with the specified ID.");
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!await _productRepository.IsProductExistAsync(id))
            {
                return NotFound("Product not found with the specified ID.");
            }
            if (await _productRepository.DeleteProductAsync(id))
                return Ok(new
                {
                    Message = "Product deleted successfully"
                });
            return BadRequest();
        }
    }
}
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RookieEShopper.Api.Dto;
using RookieEShopper.Application.Dto.Review;
using RookieEShopper.Application.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RookieEShopper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IValidator<CreateProductReviewDto> _productReviewValidator;
        public ReviewController(IReviewRepository reviewRepository, IValidator<CreateProductReviewDto> productReviewValidator) 
        {
            _productReviewValidator = productReviewValidator;
            _reviewRepository = reviewRepository;
        }
        // GET: api/<ReviewController>
        [HttpGet]
        public async Task<Results<Ok<ApiListObjectResponse<ResponseProductReviewDto>>,BadRequest>> GetProductReviewsAsync()
        {
            var productReviews = await _reviewRepository.GetAllReviewsAsync();
            return TypedResults.Ok(new ApiListObjectResponse<ResponseProductReviewDto> { 
                Data = productReviews.ToList(), Message = "Products fetched successfully", Total = productReviews.Count() 
            });
        }

        [HttpGet]
        [Route("Customer/{customerId}")]
        public async Task<Results<Ok<ApiListObjectResponse<ResponseProductReviewDto>>, BadRequest>> GetProductReviewsByCustomerAsync(Guid customerId)
        {
            var productReviews = await _reviewRepository.GetReviewsByCustomerAsync(customerId);
            return TypedResults.Ok(new ApiListObjectResponse<ResponseProductReviewDto>
            {
                Data = productReviews.ToList(),
                Message = "Products fetched successfully",
                Total = productReviews.Count()
            });
        }

        [HttpGet]
        [Route("Product/{productId}")]
        public async Task<Results<Ok<ApiListObjectResponse<ResponseProductReviewDto>>, BadRequest>> GetProductReviewsByProductAsync(int productId)
        {
            var productReviews = await _reviewRepository.GetReviewsByProductAsync(productId);
            return TypedResults.Ok(new ApiListObjectResponse<ResponseProductReviewDto>
            {
                Data = productReviews.ToList(),
                Message = "Reviews fetched successfully",
                Total = productReviews.Count()
            });
        }

        // GET api/<ReviewController>/5
        [HttpGet("{id}")]
        public async Task<Results<Ok<ResponseProductReviewDto>, BadRequest>> GetProductReviewByIdAsync(int id)
        {
            var productReviews = await _reviewRepository.GetReviewByIdAsync(id);
            return TypedResults.Ok(productReviews);
        }

        // POST api/<ReviewController>
        [HttpPost]
        [Authorize]
        public async Task<Results<Ok<ResponseProductReviewDto>, BadRequest<List<KeyValuePair<string, string[]>>>>> PostProductReview(CreateProductReviewDto createProductReviewDto)
        {
            ValidationResult validationResult = _productReviewValidator.Validate(createProductReviewDto);
            if (validationResult.IsValid)
            {
                return TypedResults.Ok(await _reviewRepository.CreateReviewAsync(createProductReviewDto));
            }
            else
            {
                return TypedResults.BadRequest(validationResult.ToDictionary().ToList());
            }
        }

        // PUT api/<ReviewController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReviewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

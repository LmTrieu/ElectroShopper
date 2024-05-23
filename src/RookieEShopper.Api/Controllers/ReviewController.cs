using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RookieEShopper.Application.Dto.Review;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RookieEShopper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository) 
        {
            _reviewRepository = reviewRepository;
        }
        // GET: api/<ReviewController>
        [HttpGet]
        public async Task<Results<Ok<ICollection<ResponseProductReviewDto>>,BadRequest>> GetProductReviewsAsync()
        {
            var productReviews = await _reviewRepository.GetAllReviewsAsync();
            return TypedResults.Ok(productReviews);
        }

        [HttpGet]
        [Route("Customer/{customerId}")]
        public async Task<Results<Ok<ICollection<ResponseProductReviewDto>>, BadRequest>> GetProductReviewsByCustomerAsync(int customerId)
        {
            var productReviews = await _reviewRepository.GetReviewsByCustomerAsync(customerId);
            return TypedResults.Ok(productReviews);
        }

        [HttpGet]
        [Route("Product/{productId}")]
        public async Task<Results<Ok<ICollection<ResponseProductReviewDto>>, BadRequest>> GetProductReviewsByProductAsync(int productId)
        {
            var productReviews = await _reviewRepository.GetReviewsByProductAsync(productId);
            return TypedResults.Ok(productReviews);
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
        public void Post([FromBody] string value)
        {
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

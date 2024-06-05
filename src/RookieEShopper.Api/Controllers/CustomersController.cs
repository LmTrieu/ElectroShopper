using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RookieEShopper.Api.Dto;
using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Repositories;
using RookieEShopper.SharedLibrary.HelperClasses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RookieEShopper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<Results<Ok<ApiListObjectResponse<ResponseCustomerDto>>, NotFound<string>>> GetProducts([FromQuery] QueryParameters query)
        {
            var result = await _customerRepository.GetAllCustomerAsync(query);

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

                return TypedResults.Ok(new ApiListObjectResponse<ResponseCustomerDto> { Data = result, Message = "Customers fetched successfully", Total = result.Count() });
            }
            return TypedResults.NotFound("No customer is available at the moment, try again later");
        }
    }
}
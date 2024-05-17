using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RookieEShopper.Application.Dto;
using RookieEShopper.Application.Repositories;

namespace RookieEShopper.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomAuthenticationController : ControllerBase
    {
        private readonly IValidator<LoginCustomerDto> _validator;
        private readonly ICustomAuthRepository _customAuthRepository;

        public CustomAuthenticationController(IValidator<LoginCustomerDto> validator, ICustomAuthRepository customAuthRepository)
        {
            _validator = validator;
            _customAuthRepository = customAuthRepository;
        }

        // Will change to Global Exception
        public class ErrorResponse
        {
            public List<ErrorDetail>? Errors { get; set; }
        }

        public class ErrorDetail
        {
            public string? FieldName { get; set; }
            public string? ErrorMessage { get; set; }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IResult> Login([FromBody] LoginCustomerDto loginRequestModel)
        {
            ValidationResult validationResult = _validator.Validate(loginRequestModel);

            if (!validationResult.IsValid)
            {
                var errorResponse = new ErrorResponse()
                {
                    Errors = validationResult.Errors.Select(e => new ErrorDetail
                    {
                        FieldName = e.PropertyName,
                        ErrorMessage = e.ErrorMessage
                    }).ToList()
                };

                return Results.BadRequest(errorResponse);
            }

            if (await _customAuthRepository.SignInUser(loginRequestModel))
            {
                return Results.Ok(await _customAuthRepository.CreateJwtUserAccessToken(loginRequestModel));
            }

            return Results.Unauthorized();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Results<Created<LoginCustomerDto>, BadRequest<ErrorResponse>>> Register([FromBody] LoginCustomerDto loginRequestModel)
        {
            ValidationResult validationResult = _validator.Validate(loginRequestModel);

            if (!validationResult.IsValid)
            {
                var errorResponse = new ErrorResponse()
                {
                    Errors = validationResult.Errors.Select(e => new ErrorDetail
                    {
                        FieldName = e.PropertyName,
                        ErrorMessage = e.ErrorMessage
                    }).ToList()
                };

                return TypedResults.BadRequest(errorResponse);
            }

            await _customAuthRepository.RegisterUser(loginRequestModel);

            return TypedResults.Created("/api/CustomAuthentication/Register", loginRequestModel);
        }

        //[HttpGet]
        //[Route("Logout")]
        //public async Task<IResult> Logout([FromBody] LoginRequestBodyDto loginRequestModel)
        //{
        //}
    }
}
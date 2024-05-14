using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using RookieEShopper.Backend.Models;
using RookieEShopper.Backend.Service.IRepositories;

namespace RookieEShopper.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomAuthenticationController : ControllerBase
    {
        private readonly IValidator<LoginRequestBodyDto> _validator;
        private readonly ICustomAuthRepository _customAuthRepository;
        public CustomAuthenticationController(IValidator<LoginRequestBodyDto> validator, ICustomAuthRepository customAuthRepository)
        {
            _validator = validator;
            _customAuthRepository = customAuthRepository;
        }

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
        public async Task<IResult> Login([FromBody] LoginRequestBodyDto loginRequestModel)
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
    }
}

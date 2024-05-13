using ElectroShopper.Data;
using ElectroShopper.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ElectroShopper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomAuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IValidator<LoginRequestBodyDto> _validator;
        public CustomAuthenticationController(IConfiguration configuration, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
                                              IValidator<LoginRequestBodyDto> validator) 
        {
            _config = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _validator = validator;
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

            var result = await _signInManager.PasswordSignInAsync(loginRequestModel.Email, loginRequestModel.Password, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes
                (_config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, loginRequestModel.Email),
                        new Claim(JwtRegisteredClaimNames.Email, loginRequestModel.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };

                var identityUser = await _userManager.FindByEmailAsync(loginRequestModel.Email);
                var roles = await _userManager.GetRolesAsync(identityUser);

                foreach (var role in roles)
                {
                    tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);

                return Results.Ok(stringToken);
            }

            return Results.Unauthorized();
        }
    }
}

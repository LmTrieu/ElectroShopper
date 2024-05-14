
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RookieEShopper.Application.Dto;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Extension.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class CustomAuthRepository : ICustomAuthRepository
    {
        private readonly IConfiguration _config;
        //Will change to BaseApplicationUser
        private readonly SignInManager<Customer> _signInManager;
        private readonly UserManager<Customer> _userManager;
        private readonly JwtOptions _jwtOptions;
        public CustomAuthRepository(IConfiguration configuration, SignInManager<Customer> signInManager, UserManager<Customer> userManager
                                    ,JwtOptions jwtOptions)
        {
            _config = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions;
        }
        public async Task<string> CreateJwtUserAccessToken(LoginRequestBodyDto loginRequestModel)
        {
            //Error here fix later
            var identityUser = await _userManager.FindByEmailAsync(loginRequestModel.Email);

            if (identityUser is null)
                throw new ArgumentNullException("Bad Login");

            var issuer = _jwtOptions.Issuer;
            var audience = _jwtOptions.Audience;
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, loginRequestModel.Email),
                    new Claim(JwtRegisteredClaimNames.Email, loginRequestModel.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var roles = await _userManager.GetRolesAsync(identityUser);

            foreach (var role in roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }

        public async Task<bool> SignInUser(LoginRequestBodyDto loginRequestModel)
        {
            return
                await _signInManager.PasswordSignInAsync(loginRequestModel.Email, loginRequestModel.Password, false, lockoutOnFailure: true)
                    is not null;
        }
    }
}

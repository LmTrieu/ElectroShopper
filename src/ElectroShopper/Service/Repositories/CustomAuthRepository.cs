using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RookieEShopper.Backend.Models;
using RookieEShopper.Backend.Service.IRepositories;

namespace RookieEShopper.Backend.Service.Repositories
{
    public class CustomAuthRepository : ICustomAuthRepository
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public CustomAuthRepository(IConfiguration configuration, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _config = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<string> CreateJwtUserAccessToken(LoginRequestBodyDto loginRequestModel)
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

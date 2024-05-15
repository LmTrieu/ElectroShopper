
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RookieEShopper.Application.Dto;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Extension.JwtBearer;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class CustomAuthRepository : ICustomAuthRepository
    {
        private readonly SignInManager<BaseApplicationUser> _signInManager;
        private readonly UserManager<BaseApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        public CustomAuthRepository(SignInManager<BaseApplicationUser> signInManager, UserManager<BaseApplicationUser> userManager
                                    ,JwtOptions jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions;
        }
        public async Task<string> CreateJwtUserAccessToken(LoginRequestBodyDto loginRequestModel)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginRequestModel.Email);

            //Will separate this check later

            if (identityUser is null)
                throw new ArgumentNullException(loginRequestModel.Email);

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
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_jwtOptions.Key)),
                    SecurityAlgorithms.HmacSha256)
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
        public async Task<IdentityResult> RegisterUser(LoginRequestBodyDto registerRequestBodyDto)
        {
            var user = new BaseApplicationUser { Email = registerRequestBodyDto.Email, UserName = registerRequestBodyDto.Email };
            var result = await _userManager.CreateAsync(user, registerRequestBodyDto.Password );
            return result;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RookieEShopper.Application.Dto.Customer;
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
        private readonly SignInManager<BaseApplicationUser> _signInManager;
        private readonly UserManager<BaseApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        protected ILookupNormalizer _normalizer;

        public CustomAuthRepository(SignInManager<BaseApplicationUser> signInManager, UserManager<BaseApplicationUser> userManager
                                    , JwtOptions jwtOptions, ILookupNormalizer normalizer)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions;
            _normalizer = normalizer;
        }

        public async Task<string> CreateJwtUserAccessToken(LoginCustomerDto loginRequestModel)
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

        public async Task<bool> SignInUser(LoginCustomerDto loginRequestModel)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                loginRequestModel.Email, loginRequestModel.Password, false, lockoutOnFailure: false);

            return signInResult.Succeeded;
        }

        public async Task<bool> RegisterUser(LoginCustomerDto registerRequestBodyDto)
        {
            if (await IsEmailTaken(registerRequestBodyDto.Email))
                throw new ArgumentNullException(nameof(registerRequestBodyDto), "Email already taken");

            var result = await _userManager.CreateAsync(
                new BaseApplicationUser
                {
                    Email = registerRequestBodyDto.Email,
                    NormalizedEmail = _normalizer.NormalizeEmail(registerRequestBodyDto.Email),
                    UserName = registerRequestBodyDto.Email,
                    NormalizedUserName = _normalizer.NormalizeName(registerRequestBodyDto.Email),
                    Customer = new Customer { EWallet = 0 }
                },
                registerRequestBodyDto.Password);

            return result.Succeeded;
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public Task<bool> LogOutUser(string token)
        {
            throw new NotImplementedException();
        }
    }
}
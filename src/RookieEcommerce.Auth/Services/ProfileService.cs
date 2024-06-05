using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using RookieEcommerce.Auth.Models;
using System.Security.Claims;

namespace RookieEcommerce.Auth.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<BaseApplicationUser> _userManager;

        public ProfileService(
            UserManager<BaseApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            var claims = new List<Claim>
            {
                new Claim("customer.id",user.CustomerId.ToString()),
                new Claim("account.role",_userManager.GetRolesAsync(user).ToString())
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null) && user.IsActive;
        }
    }
}
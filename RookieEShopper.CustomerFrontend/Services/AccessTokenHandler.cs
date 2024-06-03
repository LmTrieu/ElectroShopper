using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace RookieEShopper.CustomerFrontend.Services
{
    public class AccessTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccessTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            // Logic to retrieve access token from user session or cookie
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.HasClaim(c => c.Type == "access_token"))
            {
                var accessToken = user.FindFirstValue("access_token");
                // Check token expiration here (similar to OnTokenValidated)
                if (ValidateTokenExpiration(accessToken)) // Implement token validation logic
                {
                    return accessToken;
                }
            }
            return null;
        }

        private bool ValidateTokenExpiration(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = "rookie.customer",
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out validatedToken);
                return true; // Token is valid
            }
            catch (SecurityTokenException ex)
            {
                if (ex.Message.Contains("Expired token"))
                {
                    return false; // Token is expired
                }
                throw; // Re-throw other exceptions
            }
        }

    }
}

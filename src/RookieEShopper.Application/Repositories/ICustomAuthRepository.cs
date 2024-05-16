using Microsoft.AspNetCore.Identity;
using RookieEShopper.Application.Dto;

namespace RookieEShopper.Application.Repositories
{
    public interface ICustomAuthRepository
    {
        Task<string> CreateJwtUserAccessToken(LoginRequestBodyDto loginRequestModel);
        Task<bool> SignInUser(LoginRequestBodyDto loginRequestModel);
        Task<IdentityResult> RegisterUser(LoginRequestBodyDto registerRequestBodyDto);

    }
}

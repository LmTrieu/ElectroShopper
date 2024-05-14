using RookieEShopper.Backend.Models;

namespace RookieEShopper.Backend.Service.IRepositories
{
    public interface ICustomAuthRepository
    {
        Task<string> CreateJwtUserAccessToken(LoginRequestBodyDto loginRequestModel);
        Task<bool> SignInUser(LoginRequestBodyDto loginRequestModel);
    }
}

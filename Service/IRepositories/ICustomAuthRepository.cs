using ElectroShopper.Models;

namespace ElectroShopper.Service.IRepositories
{
    public interface ICustomAuthRepository
    {
        Task<string> CreateJwtUserAccessToken(LoginRequestBodyDto loginRequestModel);
        Task<bool> SignInUser(LoginRequestBodyDto loginRequestModel);
    }
}

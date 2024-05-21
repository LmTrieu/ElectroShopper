using RookieEShopper.Application.Dto.Customer;

namespace RookieEShopper.Application.Repositories
{
    public interface ICustomAuthRepository
    {
        Task<string> CreateJwtUserAccessToken(LoginCustomerDto loginRequestModel);

        Task<bool> SignInUser(LoginCustomerDto loginRequestModel);

        Task<bool> RegisterUser(LoginCustomerDto registerRequestBodyDto);

        Task<bool> IsEmailTaken(string username);

        Task<bool> LogOutUser(string token);
    }
}
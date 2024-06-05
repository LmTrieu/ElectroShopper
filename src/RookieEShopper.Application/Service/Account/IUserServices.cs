namespace RookieEShopper.Application.Service.Account
{
    public interface IUserServices
    {
        Task<bool> IsCustomerExist(Guid customerId);

        Task EnsureUserExistsAsync(Guid customerId, string username, string email, string role);
    }
}
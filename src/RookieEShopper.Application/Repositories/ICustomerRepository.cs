using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomerAsync();

        Task<Customer> GetCustomerByIdAsync(int id);

        Task<Customer> GetCustomerByNameAsync(string name);

        Task<Customer> CreateCustomerAsync (Customer customer);

        Task LockCustomerAsync(int id);

        Task IsCustomerExistAsync(int id);

    }
}
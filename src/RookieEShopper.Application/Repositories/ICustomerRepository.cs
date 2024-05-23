using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<ResponseCustomerDto?>> GetAllCustomerAsync();

        Task<ResponseCustomerDto?> GetCustomerByIdAsync(int id);

        Task<Customer?> GetDomainCustomerByIdAsync(int id);

        Task<ResponseCustomerDto?> GetCustomerByNameAsync(string name);

        Task<ResponseCustomerDto?> CreateCustomerAsync (CreateCustomerDto customer);

        Task LockCustomerAsync(int id);

        Task IsCustomerExistAsync(int id);

    }
}
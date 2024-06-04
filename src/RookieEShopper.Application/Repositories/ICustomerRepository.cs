using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;

namespace RookieEShopper.Application.Repositories
{
    public interface ICustomerRepository
    {
        Task<PagedList<ResponseCustomerDto?>> GetAllCustomerAsync(QueryParameters query);

        Task<ResponseCustomerDto?> GetCustomerByIdAsync(Guid id);

        Task<Customer?> GetDomainCustomerByIdAsync(int id);

        Task<ResponseCustomerDto?> GetCustomerByNameAsync(string name);

        Task<ResponseCustomerDto?> CreateCustomerAsync (CreateCustomerDto customer);

        Task LockCustomerAsync(int id);

        Task IsCustomerExistAsync(int id);

    }
}
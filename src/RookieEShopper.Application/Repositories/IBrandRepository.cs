using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllBrandAsync();

        Task<Brand> GetBrandByIdAsync(int id);

        Task<Brand> GetBrandByNameAsync(string name);    

        Task<Brand> CreateBrandAsync(Brand brand);

        Task SetBrandLockStatusAsync(Boolean IsLock);
        
        Task<bool> IsBrandExistAsync(int id);
    }
}
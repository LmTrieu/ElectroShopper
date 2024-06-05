using RookieEShopper.Application.Dto.Brand;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllBrandAsync();

        Task<Brand> GetBrandByIdAsync(int id);

        Task<Brand> GetBrandByNameAsync(string name);

        Task<Brand> CreateBrandAsync(CreateBrandDto brandDto);

        Task SetBrandLockStatusAsync(int id, bool IsLock);

        Task<bool> IsBrandExistAsync(int id);

        Task DeleteBrandAsync(int id);

        Task UpdateBrandAsync(Brand brand);
    }
}
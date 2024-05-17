using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        public Task<Brand> CreateBrand(Brand brand)
        {
            throw new NotImplementedException();
        }

        public Task<Brand> GetBrandById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Brand> GetBrandByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsBrandExist(int id)
        {
            throw new NotImplementedException();
        }

        public Task SetBrandLockStatus(bool IsLock)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Brand>> IBrandRepository.GetAllBrandAsync()
        {
            throw new NotImplementedException();
        }
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.Brand;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BrandRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Brand> CreateBrandAsync(CreateBrandDto brandDto)
        {
            var brand = new Brand();
            _mapper.Map(brandDto, brand);

            var newBrandEntityEntry = await _context.Brands.AddAsync(brand);

            await _context.SaveChangesAsync();

            return newBrandEntityEntry.Entity;
        }

        public Task DeleteBrandAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Brand>> GetAllBrandAsync()
        {
            return await _context.Brands
                .ToListAsync();
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _context.Brands
                .FindAsync(id);
        }

        public async Task<Brand?> GetBrandByNameAsync(string name)
        {
            return await _context.Brands
                .Where(b => b.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsBrandExistAsync(int id)
        {
            return await _context.Brands
                .FindAsync(id) is null;
        }

        public async Task SetBrandLockStatusAsync(int id, bool IsLock)
        {
            var brand = await _context.Brands
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (brand is not null)
                brand.IsLocked = IsLock;
        }

        public Task UpdateBrandAsync(Brand brand)
        {
            throw new NotImplementedException();
        }
    }
}
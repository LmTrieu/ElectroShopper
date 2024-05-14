using AutoMapper;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Backend.Models;
using RookieEShopper.Backend.Data;
using RookieEShopper.Backend.Service.IRepositories;
using RookieEShopper.Backend.Data.Entities;

namespace RookieEShopper.Backend.Service.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public ProductRepository(ApplicationDbContext context, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _context = context;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);
            return product;
        }

        public async Task<List<Product>> GetProductByNameAsync(string productName)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(productName))
                .ToListAsync();
            return product;
        }
        public async Task<Product?> CreateProductAsync(ProductRequestBodyDto productdto)
        {
            var product = _mapper.Map<ProductRequestBodyDto, Product>(productdto);
            product.Category = await _categoryRepository.GetCategoryByIdAsync(productdto.CategoryId);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> UpdateProductAsync(int id, ProductRequestBodyDto productdto)
        {
            Product? result = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            _mapper.Map(productdto, result);

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<bool> IsProductExist(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            try
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync() is 1;
            }
            catch
            {
                return false;
            }
        }
    }
}

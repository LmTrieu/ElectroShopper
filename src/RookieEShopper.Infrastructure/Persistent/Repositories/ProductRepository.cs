using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Services;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly FileService _fileService;

        private string folderPath;

        public ProductRepository(ApplicationDbContext context, IMapper mapper, ICategoryRepository categoryRepository,
            FileService fileService, IWebHostEnvironment env, IBrandRepository brandRepository)
        {
            _context = context;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _fileService = fileService;
            _brandRepository = brandRepository;

            folderPath = env.ContentRootPath + "\\wwwroot\\ProductImages\\";
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

        public async Task<Product?> CreateProductAsync(CreateProductDto productdto)
        {
            var product = _mapper.Map<CreateProductDto, Product>(productdto);
            product.Category = await _categoryRepository.GetCategoryByIdAsync(productdto.CategoryId);        
            
            await _context.Products.AddAsync(product);

            product.Brand = await _brandRepository.GetBrandByIdAsync(productdto.BrandId);
            
            //product.AppliableCoupons
            
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> UpdateProductAsync(int id, CreateProductDto productdto)
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

        public async Task UploadProductImage(int id, IFormFile image)
        {
            folderPath += id;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var imagePath = folderPath + "\\" + Guid.NewGuid().ToString() + "_" + image.FileName;

            var product = await _context.Products.FindAsync(id);

            if (product is null)
                throw new ArgumentNullException(product.ToString());

            await _fileService.uploadImage(imagePath, image);

            product.ImagePath = imagePath.Replace(folderPath, "").TrimStart('\\');
            await _context.SaveChangesAsync();
        }
    }
}
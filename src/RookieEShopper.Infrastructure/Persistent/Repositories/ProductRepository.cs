using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Dto;
using Microsoft.AspNetCore.Http;
using RookieEShopper.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;


namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly FileService _fileService;

        private string folderPath;

        public ProductRepository(ApplicationDbContext context, IMapper mapper, ICategoryRepository categoryRepository,
            FileService fileService, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _fileService = fileService;

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

        public async Task UploadProductImage(int id,IFormFile image)
        {
            folderPath += id;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var imagePath = folderPath + "\\" + Guid.NewGuid().ToString()+ "_" + image.FileName;

            var product = await _context.Products.FindAsync(id);

            if (product is null)
                throw new ArgumentNullException(product.ToString());            

            await _fileService.uploadImage(imagePath,image);

            product.imagePath = imagePath.Replace(folderPath, "").TrimStart('\\');                
            await _context.SaveChangesAsync();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Services;
using static System.Net.Mime.MediaTypeNames;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly FileService _fileService;
        private readonly IWebHostEnvironment _env;


        public ProductRepository(ApplicationDbContext context, IMapper mapper, ICategoryRepository categoryRepository,
            FileService fileService, IWebHostEnvironment env, IBrandRepository brandRepository)
        {
            _context = context;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _fileService = fileService;
            _brandRepository = brandRepository;
            _env = env;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetDomainProductByIdAsync(int productId)
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

        public async Task<Product?> CreateProductAsync(CreateProductDto productdto, IFormFileCollection? galleryImages)
        {
            var product = _mapper.Map<CreateProductDto, Product>(productdto);

            product.Category = await _categoryRepository.GetCategoryByIdAsync(productdto.CategoryId);

            product.Brand = await _brandRepository.GetBrandByIdAsync(productdto.BrandId);

            await _context.Products.AddAsync(product);

            product.MainImagePath = await UploadProductImageAsync(product, productdto.ProductImage);

            foreach (var image in galleryImages.ToList())
            {
                product.ImageGallery.Add(await UploadProductImageAsync(product, image));
            }

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

        public async Task<bool> IsProductExistAsync(int id)
        {
            return await _context.Products
                .AnyAsync(e => e.Id == id);
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

        public async Task<string> UploadProductImageAsync(Product product, IFormFile image)
        {
            var folderPath = _env.ContentRootPath + "\\wwwroot\\ProductImages\\" + product.Id;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var imagePath = folderPath + "\\" + Guid.NewGuid().ToString() + "_" + image.FileName;

            await _fileService.uploadImage(imagePath, image);

            return imagePath.Replace(folderPath, "").TrimStart('\\');

        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.Category.Id == categoryId)
                .ToListAsync();
        }

        public async Task<ResponseProductDto?> GetProductByIdAsync(int productId)
        {
            ResponseProductDto product = new ResponseProductDto();

            _mapper.Map(await _context.Products.FindAsync(productId),product);
            return product;
        }
    }
}
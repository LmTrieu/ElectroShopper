using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Services;
using RookieEShopper.SharedLibrary.HelperClasses;
using RookieEShopper.SharedLibrary.ViewModels;

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

        public async Task<PagedList<ResponseProductDto>> GetAllProductsAsync(QueryParameters query)
        {
            var result = _context.Products
                .Join(
                    _context.Inventories,
                    p => p.Id,
                    i => i.Product.Id,
                    (p, i) => new
                    {
                        product = p,
                        numOfProduct = i.StockAmmount,
                    })
                .Select(p => new ResponseProductDto
                {
                    id = p.product.Id,
                    description = p.product.Description,
                    name = p.product.Name,
                    price = p.product.Price,
                    mainImagePath = p.product.MainImagePath,
                    category = _mapper.Map<CategoryVM>(p.product.Category),
                    numOfProduct = p.numOfProduct
                });

            return await PagedList<ResponseProductDto>.ToPagedList(result,
                    query.PageNumber,
                    query.PageSize);
        }

        public async Task<ResponseDomainProductDto?> GetProductDetailByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductReviews)
                .Join(
                    _context.Inventories,
                    p => p.Id,
                    i => i.Product.Id,
                    (p, i) => new
                    {
                        product = p,
                        numOfProduct = i.StockAmmount,
                    })
                    .Select(p => new ResponseDomainProductDto
                        {
                            Id = p.product.Id,
                            Name = p.product.Name,
                            Price = p.product.Price,
                            MainImagePath = p.product.MainImagePath,
                            ImageGallery = p.product.ImageGallery,
                            Description = p.product.Description,
                            ProductReviews = p.product.ProductReviews.Select(pr =>pr.Id).ToList(),
                            NumOfProduct = p.numOfProduct,
                            Category = _mapper.Map<CategoryVM>(p.product.Category)
                    })
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
            await _context.SaveChangesAsync();

            await CreateInventoryAsync(product, productdto.NumOfProduct);            

            await InitialUploadImageLogic(product, productdto.ProductImage, galleryImages);

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateProductAsync(int id, CreateProductDto productdto)
        {
            Product? result = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            await UpdateInventoryAsync(id, productdto.NumOfProduct);

            _mapper.Map(productdto, result);

            result.Category = await _context.Categories
                .FirstOrDefaultAsync(p => p.Id == productdto.CategoryId);

            result.Brand = await _context.Brands
                .FirstOrDefaultAsync(p => p.Id == productdto.BrandId);

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<bool> IsProductExistAsync(int id)
        {
            return await _context.Products
                .AnyAsync(e => e.Id == id);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                _context.Products.Remove(await _context.Products.FindAsync(id));
                return await _context.SaveChangesAsync() > 0;
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

        public async Task<PagedList<ResponseProductDto>> GetProductsByCategoryAsync(QueryParameters query,int categoryId)
        {
            var result3 = _context.Products
                .Where(p => p.Category.Id == categoryId);

            var result2 = _context.Products
                .Where(p => p.Category.Id == categoryId)
                .Join(
                    _context.Inventories,
                    p => p.Id,
                    i => i.Product.Id,
                    (p, i) => new
                    {
                        product = p,
                        numOfProduct = i.StockAmmount,
                    });
            var result = _context.Products
                .Where(p => p.Category.Id == categoryId)
                .Join(
                    _context.Inventories,
                    p => p.Id,
                    i => i.Product.Id,
                    (p, i) => new
                    {
                        product = p,
                        numOfProduct = i.StockAmmount,
                    })
                .Select(p => new ResponseProductDto
                {
                    id = p.product.Id,
                    description = p.product.Description,
                    name = p.product.Name,
                    price = p.product.Price,
                    mainImagePath = p.product.MainImagePath,
                    category = _mapper.Map<CategoryVM>(p.product.Category),
                    numOfProduct = p.numOfProduct
                });

            return await PagedList<ResponseProductDto>.ToPagedList(result,
            query.PageNumber,
            query.PageSize);
        }

        public async Task<ResponseProductDto?> GetProductByIdAsync(int productId)
        {
            ResponseProductDto product = new ResponseProductDto();

            _mapper.Map(await _context.Products.FindAsync(productId),product);
            return product;
        }

        public async Task<ResponseProductDto> UpdateProductInventoryAsync(int productId, int numOfProduct)
        {
            var product = await _context.Products.FindAsync(productId);

            if (await GetInventoryAsync(productId) is not null)
                await UpdateInventoryAsync(productId, numOfProduct);
            else if(product is not null)
                await CreateInventoryAsync(product, numOfProduct);

            return new ResponseProductDto();
        }

        //--- Service function starts here ---

        private async Task InitialUploadImageLogic(Product product,IFormFile? MainImage, IFormFileCollection? galleryImages)
        {
            if (MainImage is not null)
            {
                product.MainImagePath = await UploadProductImageAsync(product, MainImage);
            }
            if (galleryImages is not null)
            {
                foreach (var image in galleryImages.ToList())
                {
                    product.ImageGallery.Add(await UploadProductImageAsync(product, image));
                }
            }
        }

        private async Task<Inventory?> GetInventoryAsync(int productId)
        {
            return await _context.Inventories
                .Where(i => i.Product.Id == productId)
                .Include(i => i.Product)
                .FirstOrDefaultAsync();
        }

        private async Task CreateInventoryAsync(Product product, int numOfProduct)
        {
            await _context.Inventories.AddAsync(
                new Inventory
                {
                    Product = product,
                    StockAmmount = numOfProduct
                });
            await _context.SaveChangesAsync();
        }

        private async Task UpdateInventoryAsync(int productId, int numOfProduct)
        {
            var inventory = await GetInventoryAsync(productId);

            inventory.StockAmmount = numOfProduct;
            inventory.LastUpdated = DateTime.Now;

            _context.Inventories.Update(inventory);

            await _context.SaveChangesAsync();
        }


    }
}
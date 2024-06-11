using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Services;
using RookieEShopper.SharedLibrary.HelperClasses;
using RookieEShopper.SharedLibrary.ViewModels;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public partial class ProductRepository : IProductRepository
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
                .AsNoTracking()
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
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Select(p => new
                {
                    Product = p,
                    Category = p.Category,
                    Reviews = p.ProductReviews,
                    Inventory = _context.Inventories.FirstOrDefault(i => i.Product.Id == p.Id)
                })
                .Select(p => new ResponseDomainProductDto
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name,
                    Price = p.Product.Price,
                    MainImagePath = p.Product.MainImagePath,
                    ImageGallery = p.Product.ImageGallery,
                    Description = p.Product.Description,
                    ProductReviews = p.Reviews.Select(pr => pr.Id).ToList(),
                    NumOfProduct = p.Inventory != null ? p.Inventory.StockAmmount : 0,
                    Category = _mapper.Map<CategoryVM>(p.Category)
                })
                .FirstOrDefaultAsync();
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

        public async Task<Product?> CreateProductAsync(ProductDto productdto, IFormFileCollection? galleryImages)
        {
            var product = _mapper.Map<ProductDto, Product>(productdto);

            product.Category = await _categoryRepository.GetCategoryByIdAsync(productdto.CategoryId);
            product.Brand = await _brandRepository.GetBrandByIdAsync(productdto.BrandId);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            await CreateInventoryAsync(product, productdto.NumOfProduct);
            await _context.SaveChangesAsync();

            //Currently ReactApp doesnt use this due to it require this to be a separate endpoint
            await InitialUploadImageLogic(product, productdto.ProductImage, galleryImages);

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateProductAsync(int id, ProductDto productdto)
        {
            Product? result = await _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .FirstOrDefaultAsync();
            if (result is not null)
            {
                if (productdto.Price == 0)
                    productdto.Price = result.Price;

                _mapper.Map(productdto, result);

                if (productdto.NumOfProduct > 0)
                    await UpdateInventoryAsync(id, productdto.NumOfProduct);

                if (productdto.CategoryId != 0)
                    result.Category = await _context.Categories
                        .FirstOrDefaultAsync(p => p.Id == productdto.CategoryId);

                if (productdto.BrandId != 0)
                    result.Brand = await _context.Brands
                        .FirstOrDefaultAsync(p => p.Id == productdto.BrandId);

                //Currently ReactApp doesnt use this due to it require this to be a separate endpoint
                if (productdto.ProductImage != null)
                    result.MainImagePath = await UploadProductImageAsync(result, productdto.ProductImage);

                await _context.SaveChangesAsync();
            }

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

            var imagePath = $"{folderPath}\\{Guid.NewGuid()}_{image.FileName}";

            await _fileService.uploadImage(imagePath, image);

            return imagePath.Replace(folderPath, "").TrimStart('\\');
        }

        public async Task<ProductImageDto> UploadOnlyProductImageAsync(IFormFile image)
        {
            var folderPath = _env.ContentRootPath + "\\wwwroot\\ProductImages\\PlaceHolderPDID";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var imagePath = $"{folderPath}\\{Guid.NewGuid()}_{image.FileName}";

            await _fileService.uploadImage(imagePath, image);

            return new ProductImageDto
            {
                Uid = imagePath.Replace(folderPath, "").TrimStart('\\'),
                Url = "https:\\localhost:7265\\ProductImages\\PlaceHolderPDID" + imagePath.Replace(folderPath, ""),
                Name = image.Name,
                Status = "done"
            };
        }

        public async Task<PagedList<ResponseProductDto>> GetProductsByCategoryAsync(QueryParameters query,
            int categoryId)
        {
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

            _mapper.Map(await _context.Products.FindAsync(productId), product);
            return product;
        }

        public async Task<ResponseProductDto> UpdateProductInventoryAsync(int productId, int numOfProduct)
        {
            var product = await _context.Products.FindAsync(productId);

            if (await GetInventoryAsync(productId) is not null)
                await UpdateInventoryAsync(productId, numOfProduct);
            else if (product is not null)
                await CreateInventoryAsync(product, numOfProduct);

            return new ResponseProductDto();
        }

        //--- Service function starts here ---

        private async Task InitialUploadImageLogic(Product product, IFormFile? MainImage,
            IFormFileCollection? galleryImages)
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

            var folderPath = Path.Combine(_env.ContentRootPath, "wwwroot", "ProductImages", "PlaceHolderPDID");

            if (Directory.Exists(folderPath))
            {
                var files = Directory.EnumerateFiles(folderPath);

                foreach (var file in files)
                {
                    File.Delete(file);
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
            if (inventory is not null)
            {
                inventory.StockAmmount = numOfProduct;
                inventory.LastUpdated = DateTime.Now;
                _context.Inventories.Update(inventory);
            }

            await _context.SaveChangesAsync();
        }
    }
}
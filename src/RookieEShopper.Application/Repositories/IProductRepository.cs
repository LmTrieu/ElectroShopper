using Microsoft.AspNetCore.Http;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<ResponseProductDto?> GetProductByIdAsync(int productId);

        Task<Product?> GetDomainProductByIdAsync(int productId);

        Task<List<Product>> GetProductByNameAsync(string productName);
        
        Task<Product?> CreateProductAsync(CreateProductDto productdto, IFormFileCollection galleryImages);

        Task<Product?> UpdateProductAsync(int id, CreateProductDto productdto);

        Task<bool> DeleteProductAsync(Product product);

        Task<bool> IsProductExistAsync(int id);

        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);

        Task<string> UploadProductImageAsync(Product product, IFormFile image);

    }
}
using Microsoft.AspNetCore.Http;
using RookieEShopper.Application.Dto;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product?> GetProductByIdAsync(int productId);

        Task<List<Product>> GetProductByNameAsync(string productName);

        Task<Product?> CreateProductAsync(CreateProductDto productdto);

        Task<Product?> UpdateProductAsync(int id, CreateProductDto productdto);

        Task<bool> DeleteProductAsync(Product product);

        Task<bool> IsProductExist(int id);

        Task UploadProductImage(int id, IFormFile image);        
    }
}
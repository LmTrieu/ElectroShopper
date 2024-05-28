using Microsoft.AspNetCore.Http;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;

namespace RookieEShopper.Application.Repositories
{
    public interface IProductRepository
    {
        Task<PagedList<ResponseProductDto>> GetAllProductsAsync(QueryParameters query);

        Task<ResponseProductDto?> GetProductByIdAsync(int productId);

        Task<ResponseDomainProductDto?> GetProductDetailByIdAsync(int productId);

        Task<List<Product>> GetProductByNameAsync(string productName);
        
        Task<Product?> CreateProductAsync(CreateProductDto productdto, IFormFileCollection galleryImages);

        Task<Product?> UpdateProductAsync(int id, CreateProductDto productdto);

        Task<bool> DeleteProductAsync(int id);

        Task<bool> IsProductExistAsync(int id);

        Task<PagedList<ResponseProductDto>> GetProductsByCategoryAsync(QueryParameters query, int categoryId);

        Task<string> UploadProductImageAsync(Product product, IFormFile image);

        Task<ResponseProductDto> UpdateProductInventoryAsync(int productId, int numOfProduct);

    }
}
using ElectroShopper.Data.Entities;
using ElectroShopper.Models;

namespace ElectroShopper.Service.IRepositories
{
    public interface IProductRepository
    {

        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int productId);
        Task<List<Product>> GetProductByNameAsync(string productName);
        Task<Product?> CreateProductAsync(ProductRequestBodyDto productdto);
        Task<Product?> UpdateProductAsync(int id, ProductRequestBodyDto productdto);
        Task<bool> DeleteProductAsync(Product product);
        Task<bool> IsProductExist(int id);

    }
}

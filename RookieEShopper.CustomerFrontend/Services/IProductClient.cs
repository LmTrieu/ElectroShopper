using RookieEShopper.SharedViewModel;

namespace RookieEShopper.CustomerFrontend.Services
{
    public interface IProductClient
    {
        Task<ICollection<ProductVM>> GetProductsAsync();
        Task<ICollection<ProductVM>> GetProductsByCategoryAsync(int categoryId);
        Task<ProductVM> GetProductDetailById(int productId);
    }
}

using RookieEShopper.CustomerFrontend.Models;
using RookieEShopper.SharedViewModel;

namespace RookieEShopper.CustomerFrontend.Services.Product
{
    public interface IProductClient
    {
        Task<ICollection<ProductVM>> GetProductsAsync();
        Task<ICollection<ProductVM>> GetProductsByCategoryAsync(int categoryId);
        Task<ProductVM> GetProductDetailById(int productId);
        Task<ICollection<ProductReviewVM>> GetProductReviewsAsync(int productId);
        Task PostProductReviewAsync(CreateProductReviewDto createProductReviewVM);
    }
}

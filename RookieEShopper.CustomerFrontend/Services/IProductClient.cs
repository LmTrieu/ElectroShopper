using RookieEShopper.SharedViewModel;

namespace RookieEShopper.CustomerFrontend.Services
{
    public interface IProductClient
    {
        Task<ICollection<ProductVM>> GetProductsAsync();
    }
}

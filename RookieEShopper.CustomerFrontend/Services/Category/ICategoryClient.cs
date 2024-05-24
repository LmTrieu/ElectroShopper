using RookieEShopper.SharedViewModel;

namespace RookieEShopper.CustomerFrontend.Services.Category
{
    public interface ICategoryClient
    {
        Task<ICollection<CategoryGroupVM>> GetCategoriesAsync();
    }
}

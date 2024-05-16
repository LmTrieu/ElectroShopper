using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(int id);
        Task<string> GetCategoryNameByIdAsync(int id);
        Task<List<Category>> GetCategoriesAsync();
        Task<bool> IsCategoryExistAsync(int id);
        Task<bool> DeleteCategoryAsync(Category category);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);

    }
}

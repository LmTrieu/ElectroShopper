using RookieEShopper.Application.Dto.Category;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedViewModel;

namespace RookieEShopper.Application.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(int id);

        Task<string> GetCategoryNameByIdAsync(int id);

        Task<List<Category>> GetAllCategoriesAsync();

        Task<bool> IsCategoryExistAsync(int id);

        Task<bool> DeleteCategoryAsync(int id);

        Task<Category> CreateCategoryAsync(CategoryDto category);

        Task<Category> UpdateCategoryAsync(Category category);
    }
}
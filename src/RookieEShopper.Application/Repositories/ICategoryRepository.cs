using RookieEShopper.Application.Dto.Category;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;

namespace RookieEShopper.Application.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(int id);

        Task<string> GetCategoryNameByIdAsync(int id);

        Task<PagedList<ResponseCategoryDto>> GetAllCategoriesAsync(QueryParameters query);

        Task<bool> IsCategoryExistAsync(int id);

        Task<bool> DeleteCategoryAsync(int id);

        Task<Category> CreateCategoryAsync(CategoryDto category);

        Task<Category> UpdateCategoryAsync(int id, CategoryDto category);
    }
}
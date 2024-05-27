using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.Category;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedViewModel;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category is null ? throw new ArgumentException("Category not found with the specified ID.") : category;
        }

        public async Task<string> GetCategoryNameByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category is null ? throw new ArgumentException("Category not found with the specified ID.") : category.CartegoryName;
        }

        public async Task<Category> CreateCategoryAsync(CategoryDto category)
        {
            var entityEntry =
                await _context.Categories.AddAsync(new Category { CartegoryName = category.Name, Description = category.Description });
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
           
            return (await _context.SaveChangesAsync()) > 0;
        }

        public Task<Category> UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCategoryExistAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
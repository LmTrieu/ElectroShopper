using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;

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

        public async Task<Category> CreateCategoryAsync(string categoryName)
        {
            var entityEntry =
                await _context.Categories.AddAsync(new Category { CartegoryName = categoryName });
            return entityEntry.Entity;
        }

        public Task<bool> DeleteCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<Category> UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCategoryExistAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> CreateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
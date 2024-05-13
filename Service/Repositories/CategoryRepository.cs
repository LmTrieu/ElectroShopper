using AutoMapper;
using ElectroShopper.Data;
using ElectroShopper.Data.Entities;
using ElectroShopper.Service.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ElectroShopper.Service.Repositories
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

        public async Task<List<Category>> GetCategoriesAsync()
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

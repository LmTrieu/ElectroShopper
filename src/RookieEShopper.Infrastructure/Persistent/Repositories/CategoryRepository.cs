using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.Category;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;

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

        public async Task<PagedList<ResponseCategoryDto>> GetAllCategoriesAsync(QueryParameters query)
        {
            return await PagedList<ResponseCategoryDto>.ToPagedList(_context.Categories.ProjectTo<ResponseCategoryDto>(_mapper.ConfigurationProvider),
                    query.PageNumber,
                    query.PageSize);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category is null ? throw new ArgumentException("Category not found with the specified ID.") : category;
        }

        public async Task<string> GetCategoryNameByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category is null ? throw new ArgumentException("Category not found with the specified ID.") : category.Name;
        }

        public async Task<Category> CreateCategoryAsync(CategoryDto category)
        {
            var entityEntry =
                await _context.Categories.AddAsync(new Category { Name = category.Name, Description = category.Description });
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
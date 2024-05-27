using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.Category;
using RookieEShopper.Application.Dto.CategoryGroup;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Persistent;
using RookieEShopper.SharedViewModel;

namespace RookieEShopper.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryGroupRepository _categoryGroupRepository;

        public CategoriesController(ICategoryRepository categoryRepository, ICategoryGroupRepository categoryGroupRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryGroupRepository = categoryGroupRepository;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Results<Ok<Category>,BadRequest>> PostCategory(CategoryDto categoryDto)
        {
            var category =  await _categoryRepository.CreateCategoryAsync(categoryDto);

            if(category is not null)             
                return TypedResults.Ok(category);

            return TypedResults.BadRequest();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<Results<Ok, BadRequest>> DeleteCategory(int id)
        {
            if(await _categoryRepository.DeleteCategoryAsync(id))
                return TypedResults.Ok();
            return TypedResults.BadRequest();
        }

        [HttpPost]
        [Route("CategoryGroup")]
        public async Task<Results<CreatedAtRoute<CategoryGroup>, BadRequest<string>>> CreateCategoryGroup(CreateCategoryGroupDto createCategoryGroupDto)
        {
            CategoryGroup categoryGroup = await _categoryGroupRepository.CreateCategoryGroupAsync(createCategoryGroupDto);
            return TypedResults.CreatedAtRoute(categoryGroup);
        }

        [HttpGet]
        [Route("CategoryGroup")]
        public async Task<Ok<IEnumerable<CategoryGroup>>> GetCategoryGroups()
        {
            var categoryGroups = await _categoryGroupRepository.GetCategoryGroupsAsync();
            return TypedResults.Ok(categoryGroups);
        }
            

        private async Task<bool> CategoryExists(int id)
        {
            return await _categoryRepository.IsCategoryExistAsync(id);
        }
    }
}
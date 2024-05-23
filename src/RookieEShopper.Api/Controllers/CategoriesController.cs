using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.CategoryGroup;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Persistent;

namespace RookieEShopper.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //Context is to be removed
        private readonly ApplicationDbContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryGroupRepository _categoryGroupRepository;

        public CategoriesController(ApplicationDbContext context, ICategoryRepository categoryRepository, ICategoryGroupRepository categoryGroupRepository)
        {
            _context = context;
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

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            //To be change
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
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
            

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
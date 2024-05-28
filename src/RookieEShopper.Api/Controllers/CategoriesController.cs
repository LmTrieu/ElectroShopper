using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RookieEShopper.Api.Dto;
using RookieEShopper.Application.Dto.Category;
using RookieEShopper.Application.Dto.CategoryGroup;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Persistent.Repositories;
using RookieEShopper.SharedLibrary.HelperClasses;
using RookieEShopper.SharedLibrary.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RookieEShopper.Api.Controllers
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
        public async Task<Results<Ok<ApiListObjectResponse<ResponseCategoryDto>>, NotFound<string>>> GetCategories([FromQuery] QueryParameters query)
        {
            var result = await _categoryRepository.GetAllCategoriesAsync(query);

            if (result.Count > 0)
            {
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

                return TypedResults.Ok(new ApiListObjectResponse<ResponseCategoryDto> { Data = result, Message = "Products fetched successfully", Total = result.Count() });
            }
            return TypedResults.NotFound("No product is available at the moment, try again later");
        }

        // GET: api/Categories/5
        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<Results<Ok<ApiSingleObjectResponse<Category>>, NotFound<string>>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            return category is null ?
                TypedResults.NotFound("No product is available at the moment, try again later") :
                TypedResults.Ok(new ApiSingleObjectResponse<Category> { Data = category, Message = "Products fetched successfully" });
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Results<Ok<Category>, BadRequest>> PostCategory(CategoryDto categoryDto)
        {
            var category = await _categoryRepository.CreateCategoryAsync(categoryDto);

            if (category is not null)
                return TypedResults.Ok(category);

            return TypedResults.BadRequest();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<Results<Ok, BadRequest>> DeleteCategory(int id)
        {
            if (await _categoryRepository.DeleteCategoryAsync(id))
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
        public async Task<Results<Ok<ApiListObjectResponse<ResponseCategoryGroupDto>>, NotFound<string>>> GetCategoryGroups([FromQuery] QueryParameters query)
        {
            var categoryGroups = await _categoryGroupRepository.GetCategoryGroupsAsync(query);
            if(categoryGroups.Count() > 0) 
                return TypedResults.Ok(new ApiListObjectResponse<ResponseCategoryGroupDto> { 
                    Data = categoryGroups, 
                    Message = "Groups of categories fetched", 
                    Total = categoryGroups.Count() 
                });
            return TypedResults.NotFound("No category is avaliable");

        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _categoryRepository.IsCategoryExistAsync(id);
        }
    }
}
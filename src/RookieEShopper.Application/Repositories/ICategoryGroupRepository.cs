using RookieEShopper.Application.Dto.CategoryGroup;
using RookieEShopper.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Repositories
{
    public interface ICategoryGroupRepository
    {
        Task<IEnumerable<CategoryGroup>> GetCategoryGroupsAsync();
        Task<CategoryGroup> CreateCategoryGroupAsync(CreateCategoryGroupDto categoryGroupDto);
        Task<CategoryGroup?> UpdateCategoryListAsync(int id, UpdateCategoryListDto updateCategoryList);
        Task<CategoryGroup> DeleteCategoryGroupAsync(int id);
    }
}

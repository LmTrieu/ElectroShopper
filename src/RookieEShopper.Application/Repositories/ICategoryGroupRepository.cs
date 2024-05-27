using RookieEShopper.Application.Dto.CategoryGroup;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Repositories
{
    public interface ICategoryGroupRepository
    {
        Task<PagedList<ResponseCategoryGroupDto>> GetCategoryGroupsAsync(QueryParameters query);
        Task<CategoryGroup> CreateCategoryGroupAsync(CreateCategoryGroupDto categoryGroupDto);
        Task<CategoryGroup?> UpdateCategoryListAsync(int id, UpdateCategoryListDto updateCategoryList);
        Task<CategoryGroup> DeleteCategoryGroupAsync(int id);
    }
}

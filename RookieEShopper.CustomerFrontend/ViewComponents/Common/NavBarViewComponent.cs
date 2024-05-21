using Microsoft.AspNetCore.Mvc;
using RookieEShopper.CustomerFrontend.Services;
using RookieEShopper.SharedViewModel;

namespace RookieEShopper.CustomerFrontend.ViewComponents.Common
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly ICategoryClient _categoryClient;

        public NavBarViewComponent(ICategoryClient categoryClient)
        {
            _categoryClient = categoryClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryGroups = await GetCategoryGroups();
            return View(categoryGroups);
        }

        private async Task<IEnumerable<CategoryGroupVM?>> GetCategoryGroups()
        {
            return await _categoryClient.GetCategoriesAsync();
        }
    }
}

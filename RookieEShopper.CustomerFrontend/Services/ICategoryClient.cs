﻿using RookieEShopper.SharedViewModel;

namespace RookieEShopper.CustomerFrontend.Services
{
    public interface ICategoryClient
    {
        Task<ICollection<CategoryVM>> GetCategoriesAsync();
    }
}

using RookieEShopper.Backend.Service.IRepositories;
using RookieEShopper.Backend.Service.Repositories;

namespace RookieEShopper.Backend.Service
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICustomAuthRepository, CustomAuthRepository>();
        }
    }
}

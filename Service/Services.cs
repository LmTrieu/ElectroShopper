using ElectroShopper.Service.IRepositories;
using ElectroShopper.Service.Repositories;

namespace ElectroShopper.Service
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

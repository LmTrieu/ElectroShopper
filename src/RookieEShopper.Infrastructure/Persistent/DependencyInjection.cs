using Microsoft.Extensions.DependencyInjection;
using RookieEShopper.Infrastructure.Persistent.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Infrastructure.Extension.JwtBearer;
using Microsoft.AspNetCore.Identity;
using RookieEShopper.Infrastructure.Extension.AutoMapper;
using RookieEShopper.Infrastructure.Services;
namespace RookieEShopper.Infrastructure.Persistent
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersitence(configuration);
            services.AddJwtAuthentication(configuration);
            services.AddAutoMapperConfiguration();
            
            DbSeed.Initializer(services.BuildServiceProvider());

            return services;
        }
        public static IServiceCollection AddPersitence(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (connectionString is null)
            {
                throw new ArgumentNullException("Connection string not found");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, providerOptions =>
                {
                    providerOptions.EnableRetryOnFailure().CommandTimeout(60);
                }));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICustomAuthRepository, CustomAuthRepository>();


            return services;
        }
    }
}

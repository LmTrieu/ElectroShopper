using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Infrastructure.Extension.AutoMapper;
using RookieEShopper.Infrastructure.Extension.JwtBearer;
using RookieEShopper.Infrastructure.Persistent.Repositories;
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

        private static IServiceCollection AddPersitence(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (connectionString is null)
            {
                throw new ArgumentNullException("Connection string not found");
            }

            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, providerOptions =>
                {
                    providerOptions.EnableRetryOnFailure().CommandTimeout(60);
                }));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICustomAuthRepository, CustomAuthRepository>();

            services.AddTransient<FileService>();
            return services;
        }

        private static IServiceCollection AddUserOptions(this IServiceCollection services) 
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            return services;
        }
    }
}
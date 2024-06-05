using Microsoft.Extensions.DependencyInjection;

namespace RookieEShopper.Infrastructure.Extension.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
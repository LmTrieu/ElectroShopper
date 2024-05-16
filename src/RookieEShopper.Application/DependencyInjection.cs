using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using RookieEShopper.Application.Dto;
namespace RookieEShopper.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequestBodyDto>, LoginRequestBodyValidator>();
            services.AddScoped<IValidator<ProductRequestBodyDto>, ProductRequestBodyDtoValidator>();

            return services;
        }
    }
}

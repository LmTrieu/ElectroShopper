using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RookieEShopper.Application.Dto;

namespace RookieEShopper.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginCustomerDto>, LoginCustomerValidator>();
            services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();

            return services;
        }
    }
}
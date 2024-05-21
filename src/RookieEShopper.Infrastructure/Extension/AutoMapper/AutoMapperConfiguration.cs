using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Dto.Product;

namespace RookieEShopper.Infrastructure.Extension.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IValidator<LoginCustomerDto>, LoginCustomerValidator>();
            services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
        }
    }
}
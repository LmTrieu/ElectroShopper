using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Dto.Review;

namespace RookieEShopper.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginCustomerDto>, LoginCustomerValidator>();
            services.AddScoped<IValidator<ProductDto>, CreateProductDtoValidator>();
            services.AddScoped<IValidator<CreateProductReviewDto>, CreateProductReviewDtoValidator>();

            return services;
        }
    }
}
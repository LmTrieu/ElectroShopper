using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RookieEShopper.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Infrastructure.Extension.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IValidator<LoginRequestBodyDto>, LoginRequestBodyValidator>();
            services.AddScoped<IValidator<ProductRequestBodyDto>, ProductRequestBodyDtoValidator>();
        }
    }
}

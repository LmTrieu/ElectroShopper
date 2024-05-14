using AutoMapper;
using RookieEShopper.Backend.Data.Entities;
using RookieEShopper.Backend.Models;

namespace RookieEShopper.Backend.Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductRequestBodyDto, Product>();
            CreateMap<Product, Product>();
        }

    }
}

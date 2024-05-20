using AutoMapper;
using RookieEShopper.Application.Dto;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Backend.Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, Product>();

            CreateMap<CreateBrandDto, Brand>();
            CreateMap<UpdateBrandDto, Brand>();
        }
    }
}
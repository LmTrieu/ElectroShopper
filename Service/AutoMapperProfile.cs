using AutoMapper;
using ElectroShopper.Data.Entities;
using ElectroShopper.Models;

namespace ElectroShopper.Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductRequestBodyDto, Product>();
            CreateMap<Product,Product>();
        }

    }
}

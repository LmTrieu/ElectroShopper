using AutoMapper;
using RookieEShopper.Application.Dto.Brand;
using RookieEShopper.Application.Dto.CategoryGroup;
using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Dto.Review;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Backend.Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, Product>();
            CreateMap<Product, ResponseProductDto>();

            CreateMap<CreateProductReviewDto, ProductReview>();
            CreateMap<ProductReview, ResponseProductReviewDto>()
                .ForMember(x => x.Customer, opt => opt.Ignore())
                .ForMember(x => x.Product, opt => opt.Ignore());

            CreateMap<CreateBrandDto, Brand>();
            CreateMap<UpdateBrandDto, Brand>();

            CreateMap<CreateCategoryGroupDto, CategoryGroup>();

            CreateMap<Customer, ResponseCustomerDto>();
            CreateMap<BaseApplicationUser, ResponseCustomerDto>();
        }
    }
}
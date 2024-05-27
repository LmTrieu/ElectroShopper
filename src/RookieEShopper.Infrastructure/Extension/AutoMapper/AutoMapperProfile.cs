using AutoMapper;
using RookieEShopper.Application.Dto.Brand;
using RookieEShopper.Application.Dto.Category;
using RookieEShopper.Application.Dto.CategoryGroup;
using RookieEShopper.Application.Dto.Customer;
using RookieEShopper.Application.Dto.Product;
using RookieEShopper.Application.Dto.Review;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.ViewModels;

namespace RookieEShopper.Infrastructure.Extension.AutoMapper
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
            CreateMap<CategoryGroup, ResponseCategoryGroupDto>();
            CreateMap<Category, CategoryVM>();
            CreateMap<Category, ResponseCategoryDto>();

            CreateMap<Customer, ResponseCustomerDto>();
            CreateMap<BaseApplicationUser, ResponseCustomerDto>();
        }
    }
}
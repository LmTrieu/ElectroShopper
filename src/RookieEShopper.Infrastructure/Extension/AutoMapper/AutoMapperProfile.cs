﻿using AutoMapper;
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
            CreateMap<ProductDto, Product>()
                .ForAllMembers(opts =>
                {
                    opts.Condition((src, dest, srcMember) => srcMember != default);
                });
            CreateMap<Product, Product>();
            CreateMap<Product, ResponseProductDto>();

            CreateMap<CreateProductReviewDto, ProductReview>();
            CreateMap<ProductReview, ResponseProductReviewDto>()
                .ForMember(x => x.Customer, opt => opt.Ignore());

            CreateMap<CreateBrandDto, Brand>();
            CreateMap<UpdateBrandDto, Brand>();

            CreateMap<CreateCategoryGroupDto, CategoryGroup>();
            CreateMap<CategoryGroup, ResponseCategoryGroupDto>();
            CreateMap<Category, CategoryVM>();
            CreateMap<Category, ResponseCategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Customer, ResponseCustomerDto>();
            CreateMap<BaseApplicationUser, ResponseCustomerDto>();
        }
    }
}
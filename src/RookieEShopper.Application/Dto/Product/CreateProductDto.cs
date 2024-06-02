using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RookieEShopper.Application.Dto.Product
{
    public class ProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? ProductImage { get; set; }
        public int NumOfProduct { get; set; }
        public int BrandId { get; set; }
        public int[] AppliableCouponsId { get; set; } = Array.Empty<int>();
    }

    public class CreateProductDtoValidator : AbstractValidator<ProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThan(5000);
        }
    }
}
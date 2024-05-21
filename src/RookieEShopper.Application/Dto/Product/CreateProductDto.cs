using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RookieEShopper.Application.Dto.Product
{
    public class CreateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? ProductImage { get; set; }
        public int NumOfProduct { get; set; } = 1;
        public int BrandId { get; set; }
        public int[] AppliableCouponsId { get; set; } = Array.Empty<int>();
    }

    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        }
    }
}
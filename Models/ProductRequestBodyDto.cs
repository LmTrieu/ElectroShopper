using FluentValidation;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace ElectroShopper.Models
{
    public class ProductRequestBodyDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }

    public class ProductRequestBodyDtoValidator : AbstractValidator<ProductRequestBodyDto> 
    {
        public ProductRequestBodyDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        }
    }
}

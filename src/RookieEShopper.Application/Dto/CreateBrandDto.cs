using FluentValidation;

namespace RookieEShopper.Application.Dto
{
    public class CreateBrandDto
    {
        public string Name { get; set; }
        public int[]? CategoriesId { get; set; }
    }

    public class CreateBrandDtoValidator : AbstractValidator<CreateBrandDto>
    {
        public CreateBrandDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}

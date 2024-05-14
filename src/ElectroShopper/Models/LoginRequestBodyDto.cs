using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace RookieEShopper.Backend.Models
{
    public class LoginRequestBodyDto
    {
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    public class LoginRequestBodyValidator : AbstractValidator<LoginRequestBodyDto>
    {
        public LoginRequestBodyValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}

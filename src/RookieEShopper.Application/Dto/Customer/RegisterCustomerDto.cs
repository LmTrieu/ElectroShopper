using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace RookieEShopper.Application.Dto.Customer
{
    public class RegisterCustomerDto
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    public class RegisterCustomerValidator : AbstractValidator<RegisterCustomerDto>
    {
        public RegisterCustomerValidator()
        {
            RuleFor(x => x.Email)
              .NotEmpty()
              .EmailAddress()
              .WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
              .NotEmpty()
              .MinimumLength(6)
              .WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.UserName)
              .NotEmpty()
              .Length(4, 20)
              .Matches("^[a-zA-Z0-9_]*$")
              .WithMessage("Username must be 4-20 characters and contain only letters, numbers, or underscores.");
        }
    }
}
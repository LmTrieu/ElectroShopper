using System.ComponentModel.DataAnnotations;

namespace RookieEcommerce.Auth.Models.Dtos
{
    public class RegisterCustomerDto
    {
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}

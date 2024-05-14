using Microsoft.AspNetCore.Identity;

namespace RookieEShopper.Domain.Data.Entities
{
    public class BaseApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? Address { get; set; }
    }
}

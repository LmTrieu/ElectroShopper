using Microsoft.AspNetCore.Identity;

namespace ElectroShopper.Data.Entities
{
    public class Customer : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? Address { get; set; }

    }
}

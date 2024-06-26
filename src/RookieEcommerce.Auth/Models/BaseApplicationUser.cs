﻿using Microsoft.AspNetCore.Identity;

namespace RookieEcommerce.Auth.Models
{
    public class BaseApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? Address { get; set; }
        public bool IsLocked { get; set; }

        public Guid CustomerId { get; set; }
    }
}
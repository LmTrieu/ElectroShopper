using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RookieEcommerce.Auth.Models
{
    public class IdentityServerFDbContext : IdentityDbContext<BaseApplicationUser>
    {
        public IdentityServerFDbContext()
        {

        }

        public IdentityServerFDbContext(DbContextOptions<IdentityServerFDbContext> options)
            : base(options)
        {
        }

    }
}

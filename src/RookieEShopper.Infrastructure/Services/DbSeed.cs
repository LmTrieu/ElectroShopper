using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Infrastructure.Services
{
    public static class DbSeed
    {
        public static async void Initializer(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            string[] roles = new string[] { "Customer", "Administrator", "Seller" };

            foreach (string role in roles)
            {
                if (roleManager.Roles.Where(r => r.Name == role).IsNullOrEmpty())
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //var Customer = new BaseApplicationUser
            //{
            //    Email = "customerA@gmail.com"
            //};

            //if (!context.Users.Any(u => u.UserName == Customer.Email))
            //{
            //    var password = new PasswordHasher<BaseApplicationUser>();
            //    var hashed = password.HashPassword(Customer, "Abc@321");
            //    Customer.PasswordHash = hashed;

            //    var userStore = new UserStore<BaseApplicationUser>(context);
            //    var result = userStore.CreateAsync(Customer);
            //}

            //AssignRoles(serviceProvider, Customer.Email, roles);

            //context.SaveChangesAsync();
        }

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            UserManager<BaseApplicationUser> _userManager = services.GetService<UserManager<BaseApplicationUser>>();
            BaseApplicationUser user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        }
    }
}
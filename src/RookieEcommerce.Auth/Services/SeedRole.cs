using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RookieEcommerce.Auth.Models;
using Serilog;
using System.Runtime.InteropServices;

namespace RookieEcommerce.Auth.Services
{
    public class SeedRole
    {
        public static async void EnsureSeedData(string connectionString, IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<IdentityServerFDbContext>();
                    context.Database.Migrate();

                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BaseApplicationUser>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    IdentityRole adminRole, customerRole;
                    CreateRoles(roleManager, out adminRole, out customerRole);
                    CreateAccounts(userManager, adminRole, customerRole);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        private static void CreateRoles(RoleManager<IdentityRole> roleManager, out IdentityRole? adminRole, out IdentityRole? customerRole)
        {
            adminRole = roleManager.FindByNameAsync("Admin").Result;
            if (adminRole == null)
            {
                adminRole = new IdentityRole
                {
                    Name = "Admin",
                };
                _ = roleManager.CreateAsync(new IdentityRole("Admin")).Result;

                //roleManager.AddClaimAsync(adminRole, new(JwtClaimTypes.Role, "Admin")).Wait();
            }

            customerRole = roleManager.FindByNameAsync("Customer").Result;
            if (customerRole == null)
            {
                customerRole = new IdentityRole
                {
                    Name = "Customer",
                };
                _ = roleManager.CreateAsync(new IdentityRole("Customer")).Result;

                //roleManager.AddClaimAsync(adminRole, new(JwtClaimTypes.Role, "Customer")).Wait();
            }
        }
        private static void CreateAccounts(UserManager<BaseApplicationUser> userManager, IdentityRole adminRole, IdentityRole customerRole)
        {
            var lmtrieu = userManager.FindByNameAsync("lmtrieu").Result;

            if (lmtrieu is null)
            {
                lmtrieu = new()
                {
                    UserName = "lmtrieu250902@gmail.com",
                    Email = "lmtrieu250902@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "1234567890",
                    LockoutEnabled = false
                };
                var result = userManager.CreateAsync(lmtrieu, "205051730").Result;

                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                result = userManager.AddClaimsAsync(lmtrieu,
                [
                    new(JwtClaimTypes.PhoneNumber, lmtrieu.PhoneNumber),
                    new(JwtClaimTypes.Email, lmtrieu.Email)
                ]).Result;

                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                userManager.AddToRoleAsync(lmtrieu, "Admin").Wait();

                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                Log.Debug("lmtrieu created");
            }
            else
            {
                Log.Debug("lmtrieu already exists");
            }

            var user = userManager.FindByNameAsync("user").Result;

            if (user is null)
            {
                user = new()
                {
                    UserName = "normaluser.dev@gmail.com",
                    Email = "normaluser.dev@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "1234567890",
                    LockoutEnabled = false
                };

                var result = userManager.CreateAsync(user, "NashRookie").Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                result = userManager.AddClaimsAsync(user,
                [
                    new(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new(JwtClaimTypes.Email, user.Email)
                ]).Result;

                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                userManager.AddToRoleAsync(user, "Customer").Wait();

                Log.Debug("user created");
            }
            else
            {
                Log.Debug("user already exists");
            }
        }

    }
}

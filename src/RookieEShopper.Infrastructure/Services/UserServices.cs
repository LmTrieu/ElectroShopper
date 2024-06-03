using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RookieEShopper.Application.Service.Account;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.Infrastructure.Persistent;

namespace RookieEShopper.Infrastructure.Services
{
    public class UserServices : IUserServices
    {
        private readonly ILogger<UserServices> _logger;
        private readonly UserManager<BaseApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public UserServices(ILogger<UserServices> logger, UserManager<BaseApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task EnsureUserExistsAsync( Guid customerId, string username, string email)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                user = new BaseApplicationUser { UserName = username, Email = email, Customer = new Customer { Id = customerId } };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User created: {username}");
                }
                else
                {
                    _logger.LogError($"Failed to create user: {username}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            if (user.Customer == null)
            {
                var entityEntry = await _context.Customers.AddAsync(new Customer { Id = customerId });
                              
                user.Customer = entityEntry.Entity;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsCustomerExist(Guid customerId)
        {
            return await _context.Users.Where(u => u.Customer.Id == customerId).AnyAsync();
        }
    }
}

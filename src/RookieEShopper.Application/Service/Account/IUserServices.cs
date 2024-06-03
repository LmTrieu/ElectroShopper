using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Service.Account
{
    public interface IUserServices
    {
        Task<bool> IsCustomerExist(Guid customerId);
        Task EnsureUserExistsAsync(Guid customerId, string username, string email);
    }
}

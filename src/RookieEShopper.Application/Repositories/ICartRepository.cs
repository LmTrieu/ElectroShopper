using RookieEShopper.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RookieEShopper.Application.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllCartAsync();

        Task<IEnumerable<CartItem>> GetAllCartItemAsync();

        Task<Cart?> GetCartByIdAsync(int id);

        Task<IEnumerable<Cart>> GetCartsByCustomerAsync(int customerId);

        Task<Cart> CreateCartAsync(int customerId);

        Task<Cart> AddItemAsync(int cartId, Product product, int quantity);

        Task<Cart> RemoveItemAsync(int cartItemId, int quantity);

        Task<Cart> UpdateCartItemAsync(CartItem cartItem);
    }
}

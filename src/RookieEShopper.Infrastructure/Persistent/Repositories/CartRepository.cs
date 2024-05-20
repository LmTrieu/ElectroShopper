using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context
            ) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllCartAsync()
        {
            return await _context.Carts
                .ToListAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemAsync()
        {
            return await _context.CartItems
                .ToListAsync();
        }

        public async Task<Cart> AddItemAsync(int cartId, Product product, int quantity)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart is null)
                throw new ArgumentNullException(cartId + "was not found");

            var cartItem = new CartItem()
            {
                Product = product,
                Quantity = quantity
            };
            cart.Items.Add(cartItem);

            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> CreateCartAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            customer.ShoppingCart = new Cart();
            await _context.SaveChangesAsync();

            return customer.ShoppingCart;
        }

        public async Task<Cart?> GetCartByIdAsync(int id)
        {
            return await _context.Carts
                .FindAsync(id);
        }

        public async Task<IEnumerable<Cart>> GetCartsByCustomerAsync(int customerId)
        {
            return await _context.Carts
                .Where(c => c.Customer.Id == customerId)
                .ToListAsync();
        }

        public async Task<Cart> RemoveItemAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem is not null)
                _context.Remove(cartItem);

            await _context.SaveChangesAsync();

            return cartItem.Cart; 
        }

        public Task<Cart> UpdateCartItemAsync(CartItem cartItem)
        {
            throw new NotImplementedException();
        }
    }
}

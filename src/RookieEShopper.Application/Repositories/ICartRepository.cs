﻿using RookieEShopper.Application.Dto.Cart;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllCartAsync();

        Task<IEnumerable<CartItem>> GetAllCartItemAsync();

        Task<Cart?> GetCartByIdAsync(int id);

        Task<Cart> AddItemAsync(int cartId, Product product, int quantity);

        Task<Cart> RemoveItemAsync(int cartItemId);

        Task<Cart> UpdateCartItemQuantityAsync(UpdateCartItemDto cartItem);
    }
}
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<Order> GetOrderByIdAsync(int id);

        Task<Order> GetOrderByNameAsync(string name);

        Task<Order> CreateOrderAsync (Order order);

        Task<Order> UpdateOrderAsync (Order order);

        Task<Order> CancelOrderAsync (int id);
    }
}
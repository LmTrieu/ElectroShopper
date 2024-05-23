using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<Order?> GetOrderByIdAsync(int id);
        Task<OrderDetail?> GetOrderDetailByIdAsync(int id);

        Task<Order?> CreateOrderAsync (Order order);

        Task<Order?> UpdateOrderAsync (Order order);

        Task<IEnumerable<Order?>> GetAllOrdersOfCustomer(int CustomerId);

        Task CancelOrderAsync (int id);
    }
}
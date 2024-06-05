using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Application.Repositories
{
    public interface IInventoryRepository
    {
        Task<Inventory?> GetInventoryAsync(int id);

        Task<Inventory> RemoveNumberOfStockAsync(int id, int quantity);

        Task<Inventory> AddNumberOfStockAsync(int id, int quantity);

        Task<Inventory> CreateInventoryAsync(Inventory inventory, int productId);
    }
}
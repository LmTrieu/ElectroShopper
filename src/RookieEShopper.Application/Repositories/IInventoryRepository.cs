using RookieEShopper.Domain.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

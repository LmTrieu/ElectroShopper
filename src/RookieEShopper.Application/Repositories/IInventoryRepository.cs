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

        Task<Inventory> RemoveNumberOfStock(int id, int quantity);

        Task<Inventory> AddNumberOfStock(int id, int quantity);

        Task<Inventory> CreateInventoy(Inventory inventory, int productId);
    }
}

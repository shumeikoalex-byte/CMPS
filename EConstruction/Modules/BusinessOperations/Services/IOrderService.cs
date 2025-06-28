// C:\Projects\EConstruction\EConstruction\Modules\BusinessOperations\Services\IOrderService.cs
using EConstruction.Modules.BusinessOperations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EConstruction.Modules.BusinessOperations.Services
{
    public interface IOrderService
    {
        Task<Order?> GetOrderByIdAsync(int id); // Змінено на Order?
        Task SaveOrderAsync(Order order);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();
        Task DeleteOrderAsync(int id);
    }
}
// C:\Projects\EConstruction\EConstruction\Modules\BusinessOperations\Services\OrderService.cs
using EConstruction.Modules.BusinessOperations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EConstruction.Modules.BusinessOperations.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext dbContext, ILogger<OrderService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Order?> GetOrderByIdAsync(int id) // Змінено на Order?
        {
            _logger.LogInformation($"Fetching order with ID: {id}");
            return await _dbContext.Orders
                                   .Include(o => o.Items)
                                   .Include(o => o.Vendor)
                                   .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task SaveOrderAsync(Order order)
        {
            _logger.LogInformation($"Saving order with InvoiceNumber: {order.InvoiceNumber}");
            if (order.OrderId == 0)
            {
                _dbContext.Orders.Add(order);
            }
            else
            {
                _dbContext.Orders.Update(order);
                var existingItems = await _dbContext.Items.Where(i => i.OrderId == order.OrderId).ToListAsync();
                _dbContext.Items.RemoveRange(existingItems);
                await _dbContext.SaveChangesAsync();

                foreach (var item in order.Items)
                {
                    item.OrderId = order.OrderId;
                    _dbContext.Items.Add(item);
                }
            }
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Order saved successfully.");
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            _logger.LogInformation("Fetching all orders.");
            return await _dbContext.Orders
                                   .Include(o => o.Items)
                                   .Include(o => o.Vendor)
                                   .ToListAsync();
        }

        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
        {
            _logger.LogInformation("Fetching all vendors.");
            return await _dbContext.Vendors.ToListAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            _logger.LogInformation($"Deleting order with ID: {id}");
            var order = await _dbContext.Orders.FindAsync(id);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Order deleted successfully.");
            }
        }
    }
}
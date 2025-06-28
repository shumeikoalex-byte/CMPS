// C:\Projects\EConstruction\EConstruction\Modules\BusinessOperations\ViewModels\OrderViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EConstruction.Modules.BusinessOperations.Models;
using EConstruction.Modules.BusinessOperations.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EConstruction.Modules.BusinessOperations.ViewModels
{
    public partial class OrderViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderViewModel> _logger;

        public ObservableCollection<Vendor> Vendors { get; set; } = new ObservableCollection<Vendor>();
        public ObservableCollection<Item> OrderItems { get; set; } = new ObservableCollection<Item>();

        [ObservableProperty]
        private Order _currentOrder = new Order
        {
            InvoiceNumber = string.Empty,
            // ВИПРАВЛЕНО: Змінено Id на VendorId для ініціалізації нового Vendor
            Vendor = new Vendor { VendorId = 0, VendorName = "Новий постачальник" },
            Items = new List<Item>(),
            TaxAreaCode = string.Empty,
            // Додано обов'язкові поля для Order
            DocumentDate = System.DateTime.Now, // Початкове значення для DocumentDate
            Status = OrderStatus.Open // Початкове значення для Status
        };

        public IAsyncRelayCommand SaveOrderCommand { get; }
        public IAsyncRelayCommand LoadOrderCommand { get; }
        public IRelayCommand AddNewItemCommand { get; }
        public IRelayCommand RemoveOrderItemCommand { get; }

        public OrderViewModel(IOrderService orderService, ILogger<OrderViewModel> logger)
        {
            _orderService = orderService;
            _logger = logger;

            SaveOrderCommand = new AsyncRelayCommand(SaveOrderAsync);
            LoadOrderCommand = new AsyncRelayCommand<int>(LoadOrderAsync);
            AddNewItemCommand = new RelayCommand(AddNewItem);
            RemoveOrderItemCommand = new RelayCommand(RemoveItem);

            _logger.LogInformation("OrderViewModel initialized.");
        }

        private async Task SaveOrderAsync()
        {
            _logger.LogInformation("Saving order...");
            // Перед збереженням, синхронізуємо OrderItems з CurrentOrder.Items
            CurrentOrder.Items.Clear();
            foreach (var item in OrderItems)
            {
                // ВИПРАВЛЕНО: Присвоюємо CurrentOrder властивості Order нового Item
                // Це має бути зроблено тут, а не тільки при AddNewItem,
                // оскільки Items можуть бути завантажені або змінені.
                item.Order = CurrentOrder;
                CurrentOrder.Items.Add(item);
            }
            await _orderService.SaveOrderAsync(CurrentOrder);
            _logger.LogInformation("Order saved.");
        }

        private async Task LoadOrderAsync(int orderId)
        {
            _logger.LogInformation($"Loading order {orderId}...");
            CurrentOrder = await _orderService.GetOrderByIdAsync(orderId);
            if (CurrentOrder != null)
            {
                OrderItems.Clear();
                foreach (var item in CurrentOrder.Items)
                {
                    OrderItems.Add(item);
                }
            }
            _logger.LogInformation($"Order {orderId} loaded.");
        }

        private void AddNewItem()
        {
            // ВИПРАВЛЕНО: Задаємо всі required властивості для Item
            var newItem = new Item
            {
                ItemNumber = $"ITEM-{OrderItems.Count + 1}",
                Description = "Новий товар",
                Quantity = 1,
                UnitOfMeasureCode = "PCS",
                DirectUnitCostExclTax = 0,
                TaxGroupCode = "STANDARD",
                // ВИПРАВЛЕНО: Обов'язково присвоюємо об'єкт Order
                // Важливо: На цьому етапі OrderId може бути 0,
                // але сам об'єкт Order має бути наданий.
                Order = CurrentOrder
            };
            OrderItems.Add(newItem);
        }

        private void RemoveItem()
        {
            if (OrderItems.Any())
            {
                OrderItems.Remove(OrderItems.Last());
            }
        }

        public async Task LoadVendorsAsync()
        {
            _logger.LogInformation("Loading vendors...");
            var vendors = await _orderService.GetAllVendorsAsync();
            Vendors.Clear();
            foreach (var vendor in vendors)
            {
                Vendors.Add(vendor);
            }
            _logger.LogInformation($"{Vendors.Count} vendors loaded.");
        }
    }
}
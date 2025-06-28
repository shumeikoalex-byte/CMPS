// C:\Users\Oleksii\VisualStudio\CMPS\EConstruction\Modules\BusinessOperations\Models\Order.cs
using System;
using System.Collections.Generic;

namespace EConstruction.Modules.BusinessOperations.Models
{
    public enum OrderStatus
    {
        Open,
        Approved,
        Closed,
        Canceled
    }

    public class Order
    {
        public int OrderId { get; set; }
        public required string InvoiceNumber { get; set; }
        public string? ShipmentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public OrderStatus Status { get; set; }

        public int VendorId { get; set; }
        public required Vendor Vendor { get; set; }

        public required ICollection<Item> Items { get; set; } = new List<Item>(); // <-- Важливо для CS9035

        public decimal SubtotalExclTax { get; set; }
        public decimal InvoiceDiscountAmount { get; set; }
        public decimal InvoiceDiscountPercentage { get; set; }
        public decimal TotalExclTax { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalInclTax { get; set; }

        public required string TaxAreaCode { get; set; }

        // Додано ContactPerson до Order, щоб відповідати XAML прив'язці.
        // Якщо це поле повинно бути завжди заповнене, додайте 'required'.
        // Якщо це просто текстове поле для введення, яке не пов'язане напряму з Contact моделі, то string? ок.
        public string? ContactPerson { get; set; }
    }
}

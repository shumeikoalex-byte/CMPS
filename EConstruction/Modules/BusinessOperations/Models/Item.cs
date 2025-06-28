// C:\Users\Oleksii\VisualStudio\CMPS\EConstruction\Modules\BusinessOperations\Models\Item.cs
namespace EConstruction.Modules.BusinessOperations.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public required string ItemNumber { get; set; } // Додано 'required'
        public required string Description { get; set; } // Додано 'required'
        public string? LocationCode { get; set; } // Додано '?', оскільки код розташування може бути необов'язковим
        public decimal Quantity { get; set; }
        public decimal ReservedQuantity { get; set; }
        public required string UnitOfMeasureCode { get; set; } // Додано 'required'
        public decimal DirectUnitCostExclTax { get; set; }
        public required string TaxGroupCode { get; set; } // Додано 'required'

        public int OrderId { get; set; } // Зовнішній ключ
        public required Order Order { get; set; } // Додано 'required', оскільки елемент без замовлення не має сенсу
    }
}

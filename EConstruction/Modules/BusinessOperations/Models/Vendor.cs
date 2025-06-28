// C:\Users\Oleksii\VisualStudio\CMPS\EConstruction\Modules\BusinessOperations\Models\Vendor.cs
using System.Collections.Generic; // Для ICollection<Contact> та ICollection<Order>

namespace EConstruction.Modules.BusinessOperations.Models
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public required string VendorName { get; set; } // Додано 'required'
        public string? ContactPerson { get; set; } // Додано '?', може бути необов'язковим
        public string? VendorNumber { get; set; } // Додано '?', може бути необов'язковим
        public string? TaxAreaCode { get; set; } // Додано '?', може бути необов'язковим
        public decimal Balance { get; set; }

        // Додаємо навігаційні властивості для зворотного зв'язку (опціонально, але корисно)
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

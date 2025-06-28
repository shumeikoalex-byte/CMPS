// C:\Users\Oleksii\VisualStudio\CMPS\EConstruction\Modules\BusinessOperations\Models\Contact.cs
namespace EConstruction.Modules.BusinessOperations.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public required string Name { get; set; } // Додано 'required'
        public string? PhoneNumber { get; set; } // Додано '?', оскільки номер може бути необов'язковим
        public required string Email { get; set; } // Додано 'required'
        public int VendorId { get; set; } // Зовнішній ключ

        public required Vendor Vendor { get; set; } // Додано 'required', оскільки контакт без постачальника не має сенсу
    }
}

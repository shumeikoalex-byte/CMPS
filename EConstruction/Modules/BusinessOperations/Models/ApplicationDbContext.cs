// C:\Users\Oleksii\VisualStudio\CMPS\EConstruction\Modules\BusinessOperations\Models\ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
// using System.Reflection.Emit; // Видалено: не потрібен
// using Windows.UI; // Видалено: не потрібен

namespace EConstruction.Modules.BusinessOperations.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets для сутностей модуля
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфігурації та обмеження
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(v => v.VendorId);
                entity.Property(v => v.VendorName).IsRequired().HasMaxLength(100);
                entity.Property(v => v.VendorNumber).HasMaxLength(50);
                // Додаємо конфігурацію для інших властивостей, якщо потрібно
                entity.Property(v => v.TaxAreaCode).HasMaxLength(10); // Припустимо макс. довжину
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(c => c.ContactId);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(100); // Email також required
                entity.Property(c => c.PhoneNumber).HasMaxLength(20); // Припустимо макс. довжину

                entity.HasOne(c => c.Vendor)
                      .WithMany(v => v.Contacts) // Вказуємо на колекцію Contacts у Vendor
                      .HasForeignKey(c => c.VendorId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.InvoiceNumber).IsRequired().HasMaxLength(50); // InvoiceNumber тепер required
                entity.Property(o => o.ShipmentNumber).HasMaxLength(50); // Припустимо макс. довжину

                entity.HasOne(o => o.Vendor)
                      .WithMany(v => v.Orders) // Вказуємо на колекцію Orders у Vendor
                      .HasForeignKey(o => o.VendorId);

                entity.HasMany(o => o.Items)
                      .WithOne(i => i.Order)
                      .HasForeignKey(i => i.OrderId);

                // Додаємо конфігурації для інших властивостей Order
                entity.Property(o => o.Status).HasConversion<string>(); // Зберігаємо enum як string
                entity.Property(o => o.TaxAreaCode).IsRequired().HasMaxLength(10); // TaxAreaCode тепер required
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(i => i.ItemId);
                entity.Property(i => i.ItemNumber).IsRequired().HasMaxLength(50); // ItemNumber тепер required
                entity.Property(i => i.Description).IsRequired().HasMaxLength(200); // Description тепер required
                entity.Property(i => i.LocationCode).HasMaxLength(50); // Припустимо макс. довжину
                entity.Property(i => i.UnitOfMeasureCode).IsRequired().HasMaxLength(10); // UnitOfMeasureCode тепер required
                entity.Property(i => i.TaxGroupCode).IsRequired().HasMaxLength(10); // TaxGroupCode тепер required
            });
        }
    }
}

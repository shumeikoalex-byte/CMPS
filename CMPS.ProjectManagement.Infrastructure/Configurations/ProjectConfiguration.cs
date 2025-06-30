using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CMPS.ProjectManagement.Domain.Aggregates;
using CMPS.ProjectManagement.Domain.Entities; // Для Task, якщо Task є сутністю

namespace CMPS.ProjectManagement.Infrastructure.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        // Налаштування таблиці
        builder.ToTable("Projects");

        // Налаштування первинного ключа
        builder.HasKey(p => p.Id);

        // Налаштування об'єктів-значень
        builder.OwnsOne(p => p.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(250);
        });

        builder.OwnsOne(p => p.Description, description =>
        {
            description.Property(d => d.Value)
                .HasColumnName("Description")
                .HasMaxLength(1000);
        });

        builder.OwnsOne(p => p.Code, code =>
        {
            code.Property(c => c.Value)
                .HasColumnName("Code")
                .IsRequired()
                .HasMaxLength(50);
            code.HasIndex(c => c.Value).IsUnique(); // Код проекту має бути унікальним
        });

        builder.OwnsOne(p => p.Schedule, schedule =>
        {
            schedule.Property(s => s.StartDate).HasColumnName("StartDate").IsRequired();
            schedule.Property(s => s.EndDate).HasColumnName("EndDate").IsRequired();
        });

        // Налаштування перерахування статусу
        builder.Property(p => p.Status)
            .HasConversion<string>(); // Зберігати Enum як рядок

        // Налаштування колекції Tasks (як частина агрегату Project)
        builder.OwnsMany(p => p.Tasks, taskBuilder =>
        {
            taskBuilder.ToTable("Tasks"); // Окрема таблиця для завдань
            taskBuilder.WithOwner().HasForeignKey("ProjectId"); // Зовнішній ключ до Project
            taskBuilder.HasKey("Id"); // Task також має свій ID

            taskBuilder.OwnsOne(t => t.Name, name =>
            {
                name.Property(n => n.Value)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(250);
            });

            taskBuilder.OwnsOne(t => t.Description, description =>
            {
                description.Property(d => d.Value)
                    .HasColumnName("Description")
                    .HasMaxLength(1000);
            });

            taskBuilder.OwnsOne(t => t.Schedule, schedule =>
            {
                schedule.Property(s => s.StartDate).HasColumnName("StartDate").IsRequired();
                schedule.Property(s => s.EndDate).HasColumnName("EndDate").IsRequired();
            });

            taskBuilder.Property(t => t.Status)
                .HasConversion<string>(); // Зберігати Enum як рядок

            // Якщо Task має AssignedToUserId, його також потрібно налаштувати
            taskBuilder.Property(t => t.AssignedToUserId);
        });

        // Забезпечення, що поля, які не допускають null, ініціалізовані приватним конструктором
        // Це стосується попереджень CS8618, які ви могли бачити раніше.
        builder.Metadata.FindNavigation(nameof(Project.Tasks))
            ?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
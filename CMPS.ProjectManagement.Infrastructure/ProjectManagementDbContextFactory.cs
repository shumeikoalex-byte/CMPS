using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration; // ДОДАЙТЕ ЦЕЙ РЯДОК
using System; // Для InvalidOperationException
using System.IO;

namespace CMPS.ProjectManagement.Infrastructure.Data
{
    public class ProjectManagementDbContextFactory : IDesignTimeDbContextFactory<ProjectManagementDbContext>
    {
        public ProjectManagementDbContext CreateDbContext(string[] args)
        {
            // Створіть конфігурацію для отримання рядка підключення.
            // Це імітує, як ваш додаток може завантажувати конфігурацію.
            // Переконайтеся, що appsettings.json знаходиться в корені рішення або в директорії, де ви запускаєте команду.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Або FileSystem.AppDataDirectory для MAUI, але для migrations зручніше використовувати поточний каталог CLI
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection"); // Перевірте назву вашого ConnectionString

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ProjectManagementDbContext>();
            // Використовуйте той самий провайдер БД, що й у вашому додатку (наприклад, SQL Server)
            optionsBuilder.UseSqlServer(connectionString);

            return new ProjectManagementDbContext(optionsBuilder.Options);
        }
    }
}
using CMPS.ProjectManagement.Domain.Entities;
using System.Threading.Tasks; // Обов'язково для асинхронних методів

namespace CMPS.ProjectManagement.Domain.Interfaces;

/// <summary>
/// Інтерфейс репозиторію для сутності Task.
/// (Може бути опущено, якщо Task завжди завантажується через Project Aggregate).
/// </summary>
public interface ITaskRepository
{
    // Явно вказуємо, що System.Threading.Tasks.Task повертає наш доменний Entities.Task
    System.Threading.Tasks.Task<Entities.Task?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddAsync(Entities.Task task, CancellationToken cancellationToken = default);
    void Update(Entities.Task task);
    void Delete(Entities.Task task);
    System.Threading.Tasks.Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
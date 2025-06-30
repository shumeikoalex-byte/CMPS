using CMPS.ProjectManagement.Domain.Aggregates;

namespace CMPS.ProjectManagement.Domain.Interfaces;

/// <summary>
/// Інтерфейс репозиторію для агрегату Project.
/// Визначає контракти для доступу та збереження проектів.
/// </summary>
public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Project project, CancellationToken cancellationToken = default);
    void Update(Project project);
    void Delete(Project project);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
using CMPS.ProjectManagement.Infrastructure.Repositories; // Або де знаходиться IProjectRepository
using CMPS.ProjectManagement.Domain.Aggregates; // Або де знаходяться ваші сутності Project
using CMPS.ProjectManagement.Domain.Interfaces; // Цей using, якщо IProjectRepository знаходиться тут
using CMPS.ProjectManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; // Потрібно для IEnumerable
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CMPS.ProjectManagement.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectManagementDbContext _dbContext;

    public ProjectRepository(ProjectManagementDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Project> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects.AnyAsync(p => p.Id == id, cancellationToken);
    }

    // ВИПРАВЛЕНО: Тип повернення змінено на Task<IEnumerable<Project>>
    public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _dbContext.Projects.AddAsync(project, cancellationToken);
    }

    public void Update(Project project)
    {
        _dbContext.Projects.Update(project);
    }

    public void Delete(Project project)
    {
        _dbContext.Projects.Remove(project);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
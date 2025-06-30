using Microsoft.EntityFrameworkCore;
using CMPS.ProjectManagement.Domain.Aggregates;
using CMPS.ProjectManagement.Domain.Entities; // Якщо Task є окремою сутністю
using System.Reflection; // Для ApplyConfigurationsFromAssembly

namespace CMPS.ProjectManagement.Infrastructure.Data;

public class ProjectManagementDbContext : DbContext
{
    public ProjectManagementDbContext(DbContextOptions<ProjectManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    // public DbSet<Task> Tasks { get; set; } // Можливо, не потрібно, якщо Task - власність Project

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Застосувати всі конфігурації сутностей з поточної збірки
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
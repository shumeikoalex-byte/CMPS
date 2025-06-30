using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CMPS.ProjectManagement.Domain.Interfaces;
using CMPS.ProjectManagement.Infrastructure.Data;
using CMPS.ProjectManagement.Infrastructure.Repositories;

namespace CMPS.ProjectManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectManagementInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ProjectManagementDbContext>(options =>
            options.UseSqlServer(connectionString)); // Використовуйте UseSqlite, якщо потрібно для MAUI

        services.AddScoped<IProjectRepository, ProjectRepository>();
        // services.AddScoped<ITaskRepository, TaskRepository>(); // Якщо TaskRepository потрібен окремо

        // Можна додати інші сервіси інфраструктури тут
        // services.AddTransient<ISomeExternalService, SomeExternalServiceImplementation>();

        return services;
    }
}
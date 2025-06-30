using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CMPS.ProjectManagement.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectManagementApplication(this IServiceCollection services)
    {
        // Реєстрація MediatR для обробки команд та запитів у цьому шарі.
        // Це сканує поточну збірку (Application Layer) для пошуку обробників (Handlers).
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Додаткові сервіси Application-шару можуть бути зареєстровані тут
        // services.AddScoped<IProjectApplicationService, ProjectApplicationService>();

        return services;
    }
}
using MediatR;
using CMPS.ProjectManagement.Domain.Aggregates;
using CMPS.ProjectManagement.Domain.ValueObjects;
using CMPS.ProjectManagement.Domain.Interfaces;
using CMPS.ProjectManagement.Application.Projects.Dtos; // Для ProjectDto
using CMPS.SharedKernel.Event; // Для IDomainEventDispatcher

namespace CMPS.ProjectManagement.Application.Projects.Commands.CreateProject;

/// <summary>
/// Обробник команди створення проекту.
/// </summary>
public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher; // Для диспетчеризації доменних подій

    public CreateProjectCommandHandler(IProjectRepository projectRepository, IDomainEventDispatcher domainEventDispatcher)
    {
        _projectRepository = projectRepository;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        // 1. Створення об'єктів-значень з вхідних даних
        var projectName = ProjectName.From(request.Name);
        var projectDescription = ProjectDescription.From(request.Description);
        var projectCode = ProjectCode.From(request.Code);
        var projectSchedule = DateRange.Create(request.StartDate, request.EndDate);

        // 2. Створення нового агрегату "Проект" (доменна логіка)
        var project = new Project(projectName, projectDescription, projectCode, projectSchedule);

        // 3. Збереження проекту через репозиторій (здійснюється в Infrastructure-шарі)
        await _projectRepository.AddAsync(project, cancellationToken);
        await _projectRepository.SaveChangesAsync(cancellationToken); // Збереження змін та публікація подій

        // 4. Диспетчеризація доменних подій, що були додані в агрегаті Project
        // Це буде викликати обробники доменних подій (наприклад, для надсилання сповіщень)
        await _domainEventDispatcher.DispatchAndClearEvents(new[] { project }, cancellationToken);

        // 5. Повернення DTO (Data Transfer Object)
        return new ProjectDto(
            project.Id,
            project.Name.Value,
            project.Description.Value,
            project.Code.Value,
            project.Schedule.StartDate,
            project.Schedule.EndDate,
            project.Status.ToString()
        );
    }
}
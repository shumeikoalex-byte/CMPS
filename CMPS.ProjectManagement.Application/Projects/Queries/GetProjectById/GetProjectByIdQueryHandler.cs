using MediatR;
using CMPS.ProjectManagement.Domain.Interfaces;
using CMPS.ProjectManagement.Application.Projects.Dtos;
using CMPS.ProjectManagement.Domain.Exceptions; // Для обробки винятків

namespace CMPS.ProjectManagement.Application.Projects.Queries.GetProjectById;

/// <summary>
/// Обробник запиту для отримання проекту за ID.
/// </summary>
public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
        {
            // У запитах часто краще повернути null або ResourceNotFoundException
            // а не доменний виняток, оскільки це не зміна стану
            throw new ProjectNotFoundException($"Project with ID {request.ProjectId} not found.");
            // Або просто: return null; залежно від вашої стратегії обробки неіснуючих ресурсів
        }

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
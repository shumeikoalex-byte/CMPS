using MediatR;
using CMPS.ProjectManagement.Application.Projects.Dtos; // Для ProjectDto

namespace CMPS.ProjectManagement.Application.Projects.Commands.CreateProject;

/// <summary>
/// Команда для створення нового проекту.
/// IRequest<ProjectDto> означає, що ця команда поверне об'єкт ProjectDto після виконання.
/// </summary>
public record CreateProjectCommand(
    string Name,
    string Description,
    string Code,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<ProjectDto>;
using MediatR;
using CMPS.ProjectManagement.Application.Projects.Dtos;

namespace CMPS.ProjectManagement.Application.Projects.Queries.GetProjectById;

/// <summary>
/// Запит для отримання проекту за його ID.
/// IRequest<ProjectDto?> означає, що запит поверне ProjectDto або null, якщо проект не знайдено.
/// </summary>
public record GetProjectByIdQuery(Guid ProjectId) : IRequest<ProjectDto?>;
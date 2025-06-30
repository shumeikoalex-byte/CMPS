namespace CMPS.ProjectManagement.Application.Projects.Dtos;

/// <summary>
/// DTO (Data Transfer Object) для представлення інформації про проект.
/// Використовується для передачі даних від Application Layer до Presentation Layer.
/// </summary>
public record ProjectDto(
    Guid Id,
    string Name,
    string Description,
    string Code,
    DateTime StartDate,
    DateTime EndDate,
    string Status
);
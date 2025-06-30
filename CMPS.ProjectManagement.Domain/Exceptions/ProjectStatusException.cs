using CMPS.SharedKernel.Exceptions;

namespace CMPS.ProjectManagement.Domain.Exceptions;

/// <summary>
/// Виняток, що виникає, коли операція неможлива через поточний статус проекту.
/// </summary>
public class ProjectStatusException : DomainException
{
    public ProjectStatusException(string message) : base(message) { }
    public ProjectStatusException(string message, Exception innerException) : base(message, innerException) { }
}
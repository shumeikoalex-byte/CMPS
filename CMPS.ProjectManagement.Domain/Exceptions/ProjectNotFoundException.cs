// CMPS.ProjectManagement.Domain\Exceptions\ProjectNotFoundException.cs
namespace CMPS.ProjectManagement.Domain.Exceptions;

public class ProjectNotFoundException : Exception
{
    public ProjectNotFoundException(Guid projectId)
        : base($"Project with Id '{projectId}' was not found.")
    {
    }

    public ProjectNotFoundException(string message)
        : base(message)
    {
    }

    public ProjectNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
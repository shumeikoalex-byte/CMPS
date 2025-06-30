using CMPS.SharedKernel.Exceptions;

namespace CMPS.ProjectManagement.Domain.Exceptions;

/// <summary>
/// Виняток, що виникає при неможливості завершити проект через незавершені залежності.
/// </summary>
public class ProjectCompletionException : DomainException
{
    public ProjectCompletionException(string message) : base(message) { }
    public ProjectCompletionException(string message, Exception innerException) : base(message, innerException) { }
}
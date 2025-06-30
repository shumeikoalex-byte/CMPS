using CMPS.SharedKernel.Exceptions;

namespace CMPS.ProjectManagement.Domain.Exceptions;

/// <summary>
/// Виняток, що виникає при спробі додати завдання, яке вже існує в проекті.
/// </summary>
public class DuplicateTaskException : DomainException
{
    public DuplicateTaskException(string message) : base(message) { }
    public DuplicateTaskException(string message, Exception innerException) : base(message, innerException) { }
}
namespace CMPS.SharedKernel.Exceptions;

/// <summary>
/// Базовий клас для доменних винятків.
/// Використовується для позначення винятків, які походять з доменної логіки.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
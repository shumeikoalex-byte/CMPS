namespace CMPS.SharedKernel.Event;

/// <summary>
/// Інтерфейс для доменних подій.
/// Доменна подія - це те, що відбулося в домені і про що інші частини системи можуть бути зацікавлені.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Час, коли відбулася подія.
    /// </summary>
    DateTime OccurredOn { get; }
}
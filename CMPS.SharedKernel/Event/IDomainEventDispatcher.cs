using System.Threading.Tasks;

namespace CMPS.SharedKernel.Event;

/// <summary>
/// Інтерфейс для диспетчера доменних подій.
/// Відповідає за публікацію доменних подій слухачам.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Асинхронно публікує вказану доменну подію.
    /// </summary>
    /// <param name="domainEvent">Доменна подія для публікації.</param>
    /// <param name="cancellationToken">Токен скасування.</param>
    Task Dispatch(IDomainEvent domainEvent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Асинхронно публікує всі доменні події, що зберігаються в коренях агрегатів.
    /// </summary>
    /// <param name="aggregateRootsWithEvents">Список агрегатів, що містять доменні події.</param>
    /// <param name="cancellationToken">Токен скасування.</param>
    Task DispatchAndClearEvents(IEnumerable<Abstractions.AggregateRoot> aggregateRootsWithEvents, CancellationToken cancellationToken = default);
}
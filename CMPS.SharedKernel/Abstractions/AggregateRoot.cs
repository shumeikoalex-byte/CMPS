using CMPS.SharedKernel.Event;

namespace CMPS.SharedKernel.Abstractions;

/// <summary>
/// Базовий клас для коренів агрегатів. Корінь агрегату є єдиною точкою доступу до інших сутностей
/// та об'єктів-значень у межах агрегату, забезпечуючи їх консистентність.
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Список доменних подій, які відбулися з цим агрегатом.
    /// Ці події можуть бути опубліковані після успішної транзакції.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Конструктор за замовчуванням.
    /// </summary>
    protected AggregateRoot() : base() { }

    /// <summary>
    /// Конструктор для відновлення агрегату з існуючим Id.
    /// </summary>
    protected AggregateRoot(Guid id) : base(id) { }

    /// <summary>
    /// Додає доменну подію до списку подій агрегату.
    /// </summary>
    /// <param name="eventItem">Подія домену.</param>
    protected void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    /// <summary>
    /// Очищає список доменних подій агрегату. Викликається після публікації подій.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
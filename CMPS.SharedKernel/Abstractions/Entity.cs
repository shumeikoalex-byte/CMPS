namespace CMPS.SharedKernel.Abstractions;

/// <summary>
/// Базовий клас для доменних сутностей, що мають ідентичність.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Унікальний ідентифікатор сутності.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Конструктор за замовчуванням, що генерує новий GUID для Id.
    /// </summary>
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Конструктор для відновлення сутності з існуючим Id (наприклад, з бази даних).
    /// </summary>
    protected Entity(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        if (Id == Guid.Empty || other.Id == Guid.Empty)
        {
            return false; // Обидва об'єкти не ідентифіковані або один з них
        }

        return Id == other.Id;
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
        {
            return true;
        }

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
using CMPS.SharedKernel.Abstractions;

namespace CMPS.ProjectManagement.Domain.ValueObjects;

/// <summary>
/// Об'єкт-значення для опису проекту.
/// </summary>
public class ProjectDescription : ValueObject
{
    public string Value { get; private set; }

    // Приватний конструктор для EF Core або для внутрішнього використання
    private ProjectDescription()
    {
        Value = string.Empty; // Ініціалізація для nullability
    }

    private ProjectDescription(string value)
    {
        // Опис може бути порожнім, але не null
        Value = value ?? throw new ArgumentNullException(nameof(value));
        // Можна додати інші правила валідації, наприклад, максимальну довжину
    }

    public static ProjectDescription From(string value)
    {
        return new ProjectDescription(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(ProjectDescription description) => description.Value;
    public static explicit operator ProjectDescription(string value) => From(value);
}
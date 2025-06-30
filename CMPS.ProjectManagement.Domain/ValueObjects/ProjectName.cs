using CMPS.SharedKernel.Abstractions;
using CMPS.SharedKernel.Helpers;

namespace CMPS.ProjectManagement.Domain.ValueObjects;

/// <summary>
/// Об'єкт-значення для назви проекту.
/// Містить правила валідації для назви.
/// </summary>
public class ProjectName : ValueObject
{
    public string Value { get; private set; }

    // Приватний конструктор для EF Core або для внутрішнього використання
    private ProjectName()
    {
        Value = string.Empty; // Ініціалізація для nullability
    }

    private ProjectName(string value)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));
        if (value.Length > 250) // Приклад правила: назва не може бути довшою за 250 символів
        {
            throw new ArgumentException("Project name cannot exceed 250 characters.", nameof(value));
        }
        Value = value;
    }

    public static ProjectName From(string value)
    {
        return new ProjectName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }

    // Явне або неявне перетворення для зручності
    public static implicit operator string(ProjectName name) => name.Value;
    public static explicit operator ProjectName(string value) => From(value);
}
using System.Text.RegularExpressions;
using CMPS.SharedKernel.Abstractions;
using CMPS.SharedKernel.Helpers;

namespace CMPS.ProjectManagement.Domain.ValueObjects;

/// <summary>
/// Об'єкт-значення для коду проекту.
/// Може містити специфічні правила формату коду.
/// </summary>
public class ProjectCode : ValueObject
{
    public string Value { get; private set; }

    private static readonly Regex CodeRegex = new Regex(@"^[A-Z0-9]{3,10}$"); // Приклад: 3-10 великих букв або цифр

    // Приватний конструктор для EF Core або для внутрішнього використання
    private ProjectCode()
    {
        Value = string.Empty; // Ініціалізація для nullability
    }

    private ProjectCode(string value)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));

        if (!CodeRegex.IsMatch(value))
        {
            throw new ArgumentException("Project code must be 3-10 uppercase alphanumeric characters.", nameof(value));
        }

        Value = value;
    }

    public static ProjectCode From(string value)
    {
        return new ProjectCode(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(ProjectCode code) => code.Value;
    public static explicit operator ProjectCode(string value) => From(value);
}
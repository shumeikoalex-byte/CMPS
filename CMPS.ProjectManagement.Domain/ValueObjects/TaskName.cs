using CMPS.SharedKernel.Abstractions;
using CMPS.SharedKernel.Helpers;

namespace CMPS.ProjectManagement.Domain.ValueObjects;

public class TaskName : ValueObject
{
    public string Value { get; private set; }

    private TaskName()
    {
        Value = string.Empty;
    }

    private TaskName(string value)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));
        if (value.Length > 250)
        {
            throw new ArgumentException("Task name cannot exceed 250 characters.", nameof(value));
        }
        Value = value;
    }

    public static TaskName From(string value)
    {
        return new TaskName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(TaskName name) => name.Value;
    public static explicit operator TaskName(string value) => From(value);
}
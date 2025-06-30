using CMPS.SharedKernel.Abstractions;

namespace CMPS.ProjectManagement.Domain.ValueObjects;

public class TaskDescription : ValueObject
{
    public string Value { get; private set; }

    private TaskDescription()
    {
        Value = string.Empty;
    }

    private TaskDescription(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static TaskDescription From(string value)
    {
        return new TaskDescription(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(TaskDescription description) => description.Value;
    public static explicit operator TaskDescription(string value) => From(value);
}
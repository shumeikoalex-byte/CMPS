using CMPS.SharedKernel.Abstractions;
using CMPS.SharedKernel.Helpers;

namespace CMPS.ProjectManagement.Domain.ValueObjects;

/// <summary>
/// Об'єкт-значення для діапазону дат (початок та кінець).
/// </summary>
public class DateRange : ValueObject
{
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    // Приватний конструктор для EF Core або для внутрішнього використання
    private DateRange() { }

    private DateRange(DateTime startDate, DateTime endDate)
    {
        Guard.AgainstFalse(startDate <= endDate, "Start date cannot be after end date.");

        StartDate = startDate.Date; // Зберігаємо лише дату
        EndDate = endDate.Date;     // Зберігаємо лише дату
    }

    public static DateRange Create(DateTime startDate, DateTime endDate)
    {
        return new DateRange(startDate, endDate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }

    public override string ToString()
    {
        return $"{StartDate.ToShortDateString()} - {EndDate.ToShortDateString()}";
    }
}
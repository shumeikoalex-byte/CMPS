using CMPS.SharedKernel.Abstractions;
using CMPS.SharedKernel.Helpers;
using CMPS.ProjectManagement.Domain.ValueObjects;
using CMPS.ProjectManagement.Domain.Enums; // Зберігаємо це для нашого TaskStatus

namespace CMPS.ProjectManagement.Domain.Entities;

/// <summary>
/// Доменна сутність "Завдання" (Entity). Є частиною агрегату "Проект".
/// </summary>
public class Task : Entity
{
    public TaskName Name { get; private set; }
    public TaskDescription Description { get; private set; }
    public DateRange Schedule { get; private set; }
    public CMPS.ProjectManagement.Domain.Enums.TaskStatus Status { get; private set; } // Явно вказуємо наш TaskStatus
    public Guid? AssignedToUserId { get; private set; } // Призначений користувач (Id з ActorsAndRoles)

    // Приватний конструктор для EF Core або для внутрішнього використання
    private Task() 
    {
        Name = default!;
        Description = default!;
        Schedule = default!;
        // Status зазвичай має початкове значення або default!
    }
    /// <summary>
    /// Конструктор для створення нового завдання.
    /// </summary>
    public Task(TaskName name, TaskDescription description, DateRange schedule)
    {
        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(description, nameof(description));
        Guard.AgainstNull(schedule, nameof(schedule));
        Guard.AgainstFalse(schedule.StartDate <= schedule.EndDate, "Task start date cannot be after end date.");

        Name = name;
        Description = description;
        Schedule = schedule;
        Status = CMPS.ProjectManagement.Domain.Enums.TaskStatus.Pending; // Явно вказуємо наш TaskStatus
    }

    /// <summary>
    /// Оновлює назву завдання.
    /// </summary>
    public void UpdateName(TaskName newName)
    {
        Guard.AgainstNull(newName, nameof(newName));
        Name = newName;
    }

    /// <summary>
    /// Оновлює опис завдання.
    /// </summary>
    public void UpdateDescription(TaskDescription newDescription)
    {
        Guard.AgainstNull(newDescription, nameof(newDescription));
        Description = newDescription;
    }

    /// <summary>
    /// Оновлює розклад завдання.
    /// </summary>
    public void UpdateSchedule(DateRange newSchedule)
    {
        Guard.AgainstNull(newSchedule, nameof(newSchedule));
        Guard.AgainstFalse(newSchedule.StartDate <= newSchedule.EndDate, "New task start date cannot be after end date.");
        Schedule = newSchedule;
    }

    /// <summary>
    /// Призначає завдання користувачеві.
    /// </summary>
    public void AssignTo(Guid userId)
    {
        Guard.AgainstNull(userId, nameof(userId));
        AssignedToUserId = userId;
        // Можливо, додати доменну подію TaskAssignedEvent
    }

    /// <summary>
    /// Змінює статус завдання.
    /// </summary>
    public void ChangeStatus(CMPS.ProjectManagement.Domain.Enums.TaskStatus newStatus) // Явно вказуємо наш TaskStatus
    {
        if (Status == newStatus) return;

        // Додаткова логіка переходу статусів, якщо потрібно
        // Наприклад: не можна перейти з Completed у Pending

        Status = newStatus;
        // Можливо, додати доменну подію TaskStatusChangedEvent
    }

    // Додаткові методи, наприклад, для позначення завершення завдання
    public void Complete()
    {
        if (Status == CMPS.ProjectManagement.Domain.Enums.TaskStatus.Completed) return; // Вже завершено

        ChangeStatus(CMPS.ProjectManagement.Domain.Enums.TaskStatus.Completed);
    }
}
using CMPS.SharedKernel.Abstractions;
using CMPS.SharedKernel.Helpers;
using CMPS.ProjectManagement.Domain.ValueObjects;
using CMPS.ProjectManagement.Domain.Enums;
using CMPS.ProjectManagement.Domain.Events;
using CMPS.ProjectManagement.Domain.Exceptions;

namespace CMPS.ProjectManagement.Domain.Aggregates;

/// <summary>
/// Корінь агрегату "Проект". Відповідає за цілісність та життєвий цикл проекту.
/// </summary>
public class Project : AggregateRoot
{
    public ProjectName Name { get; private set; }
    public ProjectDescription Description { get; private set; }
    public ProjectCode Code { get; private set; }
    public DateRange Schedule { get; private set; }
    public ProjectStatus Status { get; private set; }

    // Внутрішня колекція завдань, що належать цьому проекту
    private readonly List<Entities.Task> _tasks = new();
    public IReadOnlyCollection<Entities.Task> Tasks => _tasks.AsReadOnly();

    // Приватний конструктор для EF Core або для внутрішнього використання
    private Project()
    {
        // Ініціалізуйте тут властивості, що не допускають NULL,
        // або використовуйте оператор "null-forgiving" (!) якщо ви впевнені,
        // що EF Core їх завантажить.
        Name = default!; // Повідомляємо компілятору, що EF Core заповнить це
        Description = default!;
        Code = default!;
        Schedule = default!;
        // Status зазвичай має початкове значення або також default!
        // _tasks вже ініціалізовано
    }
    /// <summary>
    /// Конструктор для створення нового проекту.
    /// </summary>
    public Project(ProjectName name, ProjectDescription description, ProjectCode code, DateRange schedule)
    {
        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(description, nameof(description));
        Guard.AgainstNull(code, nameof(code));
        Guard.AgainstNull(schedule, nameof(schedule));
        Guard.AgainstFalse(schedule.StartDate <= schedule.EndDate, "Project start date cannot be after end date.");

        Name = name;
        Description = description;
        Code = code;
        Schedule = schedule;
        Status = ProjectStatus.Planning; // Початковий статус

        AddDomainEvent(new ProjectCreatedEvent(Id, Name.Value, Code.Value, Schedule.StartDate));
    }

    /// <summary>
    /// Змінює назву проекту.
    /// </summary>
    public void ChangeName(ProjectName newName)
    {
        Guard.AgainstNull(newName, nameof(newName));
        Name = newName;
    }

    /// <summary>
    /// Оновлює опис проекту.
    /// </summary>
    public void UpdateDescription(ProjectDescription newDescription)
    {
        Guard.AgainstNull(newDescription, nameof(newDescription));
        Description = newDescription;
    }

    /// <summary>
    /// Оновлює розклад проекту.
    /// </summary>
    public void UpdateSchedule(DateRange newSchedule)
    {
        Guard.AgainstNull(newSchedule, nameof(newSchedule));
        Guard.AgainstFalse(newSchedule.StartDate <= newSchedule.EndDate, "New project start date cannot be after end date.");

        if (Status == ProjectStatus.Completed || Status == ProjectStatus.Cancelled)
        {
            throw new ProjectStatusException("Cannot update schedule for completed or cancelled projects.");
        }

        Schedule = newSchedule;
    }

    /// <summary>
    /// Додає завдання до проекту.
    /// </summary>
    public void AddTask(Entities.Task task)
    {
        Guard.AgainstNull(task, nameof(task));
        if (_tasks.Any(t => t.Id == task.Id))
        {
            throw new DuplicateTaskException($"Task with ID {task.Id} already exists in project {Name.Value}.");
        }
        _tasks.Add(task);
        // Можливо, додати доменну подію TaskAddedToProjectEvent
    }

    /// <summary>
    /// Змінює статус проекту.
    /// </summary>
    public void ChangeStatus(ProjectStatus newStatus)
    {
        if (Status == newStatus) return;

        // Додаткова логіка переходу статусів, якщо потрібно
        // Наприклад: не можна перейти з Completed у Planning

        Status = newStatus;
        AddDomainEvent(new ProjectStatusChangedEvent(Id, newStatus.ToString(), DateTime.UtcNow));
    }

    // Методи для керування життєвим циклом проекту
    public void Start()
    {
        if (Status != ProjectStatus.Planning)
        {
            throw new ProjectStatusException("Project can only be started from Planning status.");
        }
        ChangeStatus(ProjectStatus.Active);
    }

    public void Complete()
    {
        if (Status != ProjectStatus.Active)
        {
            throw new ProjectStatusException("Project can only be completed from Active status.");
        }
        // Перевірка, чи всі завдання завершені, якщо це бізнес-правило
        if (_tasks.Any(t => t.Status != CMPS.ProjectManagement.Domain.Enums.TaskStatus.Completed))
        {
            throw new ProjectCompletionException("Cannot complete project: not all tasks are completed.");
        }
        ChangeStatus(ProjectStatus.Completed);
    }

    public void Cancel()
    {
        if (Status == ProjectStatus.Completed)
        {
            throw new ProjectStatusException("Cannot cancel a completed project.");
        }
        ChangeStatus(ProjectStatus.Cancelled);
    }
}
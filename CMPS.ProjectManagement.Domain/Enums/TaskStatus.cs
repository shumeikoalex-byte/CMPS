namespace CMPS.ProjectManagement.Domain.Enums;

/// <summary>
/// Перелік статусів завдання.
/// </summary>
public enum TaskStatus
{
    Pending,
    InProgress,
    Completed,
    Blocked,
    Cancelled
}
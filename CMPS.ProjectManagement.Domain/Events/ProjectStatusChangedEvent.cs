using CMPS.SharedKernel.Event;

namespace CMPS.ProjectManagement.Domain.Events;

/// <summary>
/// Доменна подія, що виникає при зміні статусу проекту.
/// </summary>
public class ProjectStatusChangedEvent : IDomainEvent
{
    public Guid ProjectId { get; }
    public string NewStatus { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProjectStatusChangedEvent(Guid projectId, string newStatus, DateTime occurredOn)
    {
        ProjectId = projectId;
        NewStatus = newStatus;
        OccurredOn = occurredOn;
    }
}
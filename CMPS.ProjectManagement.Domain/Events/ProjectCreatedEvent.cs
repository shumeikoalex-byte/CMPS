using CMPS.SharedKernel.Event;

namespace CMPS.ProjectManagement.Domain.Events;

/// <summary>
/// Доменна подія, що виникає при створенні нового проекту.
/// </summary>
public class ProjectCreatedEvent : IDomainEvent
{
    public Guid ProjectId { get; }
    public string ProjectName { get; }
    public string ProjectCode { get; }
    public DateTime ProjectStartDate { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProjectCreatedEvent(Guid projectId, string projectName, string projectCode, DateTime projectStartDate)
    {
        ProjectId = projectId;
        ProjectName = projectName;
        ProjectCode = projectCode;
        ProjectStartDate = projectStartDate;
    }
}
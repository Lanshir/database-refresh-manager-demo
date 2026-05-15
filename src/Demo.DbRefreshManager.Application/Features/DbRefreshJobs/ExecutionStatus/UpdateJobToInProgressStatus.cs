using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ExecutionStatus;

/// <summary>
/// Перевести задачу на перезаливку в статус "В процессе".
/// </summary>
public interface IUpdateJobToInProgressStatusHandler
    : IAsyncHandler<bool, UpdateJobToInProgressStatus.Command>;

public class UpdateJobToInProgressStatus
{
    public record struct Command(int JobId, string? UserComment);
}

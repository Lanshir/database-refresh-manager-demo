using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ExecutionStatus;

/// <summary>
/// Обновить задачу на перезаливку БД в статус по ум.
/// </summary>
public interface IUpdateJobToDefaultStatusHandler
    : IAsyncHandler<bool, UpdateJobToDefaultStatus.Command>;

public class UpdateJobToDefaultStatus
{
    public record struct Command(int JobId);
}

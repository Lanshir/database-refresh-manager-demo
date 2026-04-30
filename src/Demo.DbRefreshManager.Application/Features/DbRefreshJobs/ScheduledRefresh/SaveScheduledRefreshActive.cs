using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ScheduledRefresh;

/// <summary>
/// Установка активности перезаливки БД по расписанию.
/// </summary>
public interface ISaveScheduledRefreshActiveCommandHandler
    : IAsyncHandler<bool, SaveScheduledRefreshActive.Command>;

public class SaveScheduledRefreshActive
{
    /// <param name="JobId">Id задачи на перезаливку.</param>
    /// <param name="ChangeIssuerUserId">Id автора изменений.</param>
    /// <param name="IsActive">Активность перезаливки.</param>
    public record struct Command(int JobId, int ChangeIssuerUserId, bool IsActive);
}

using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.Logs;

/// <summary>
/// Логирование начала перезаливки БД.
/// </summary>
public interface ILogDbRefreshStartedHandler
    : IAsyncHandler<bool, LogDbRefreshStarted.Command>;

public class LogDbRefreshStarted
{
    /// <param name="JobId">Id задачи на перезаливку.</param>
    /// <param name="RefreshStartDate">Дата/время начала перезаливки.</param>
    /// <param name="Initiator">Инициатор перезаливки.</param>
    /// <param name="ExecutedScript">Выполненный скрипт.</param>
    public record struct Command(
        int JobId,
        DateTime RefreshStartDate,
        string Initiator,
        string ExecutedScript);
}

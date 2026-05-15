using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.Logs;

/// <summary>
/// Логирование окончания перезаливки БД.
/// </summary>
public interface ILogDbRefreshFinishedHandler
    : IAsyncHandler<bool, LogDbRefreshFinished.Command>;

public class LogDbRefreshFinished
{
    /// <param name="JobId">Id задачи на перезаливку.</param>
    /// <param name="RefreshStartDate">Дата/время начала перезаливки.</param>
    /// <param name="Code">Код результата.</param>
    /// <param name="Result">Результат.</param>
    /// <param name="Error">Ошибка.</param>
    public record struct Command(
        int JobId,
        DateTime RefreshStartDate,
        int? Code,
        string? Result,
        string? Error);
}

using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;

namespace Demo.DbRefreshManager.Application.Features.Logs;

/// <summary>
/// Получить список логов для отображения пользователю.
/// </summary>
public interface IGetLogsForDisplayHandler
    : IHandler<IQueryable<DbRefreshLog>, GetLogsForDisplay.Query>;

public class GetLogsForDisplay
{
    /// <param name="JobId">Фильтр по id задачи на перезаливку.</param>
    /// <param name="StartDate">Фильтр по дате начала перезаливки.</param>
    public record struct Query(int? JobId, DateTime? StartDate);
}

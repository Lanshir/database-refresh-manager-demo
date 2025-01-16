using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Dal.Repositories.Abstract.Base;

namespace Demo.DbRefreshManager.Dal.Repositories.Abstract;

/// <summary>
/// Репозиторий логов перезаливки БД.
/// </summary>
public interface IDbRefreshLogsRepository : IRepository<DbRefreshLog>
{
    /// <summary>
    /// Запрос логов для отображения пользователю.
    /// </summary>
    /// <param name="jobId">Фильтр по id задачи на перезаливку.</param>
    /// <param name="startDate">Фильтр по дате начала перезаливки.</param>
    IQueryable<DbRefreshLog> GetUserDisplayLogsQuery(int? jobId = null, DateTime? startDate = null);

    /// <summary>
    /// Логирование начала перезаливки БД.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <param name="refreshStartDate">Дата/время начала перезаливки.</param>
    /// <param name="initiator">Инициатор перезаливки.</param>
    /// <param name="executedScript">Выполненный скрипт.</param>
    Task LogDbRefreshStart(int jobId, DateTime refreshStartDate, string initiator, string executedScript);

    /// <summary>
    /// Логирование окончания перезаливки БД.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <param name="refreshStartDate">Дата/время начала перезаливки.</param>
    /// <param name="code">Код результата.</param>
    /// <param name="result">Результат.</param>
    /// <param name="error">Ошибка.</param>
    Task LogDbRefreshFinish(int jobId, DateTime refreshStartDate, int? code, string? result, string? error);

    /// <summary>
    /// Очистка устаревших логов.
    /// </summary>
    Task CleanOld();
}

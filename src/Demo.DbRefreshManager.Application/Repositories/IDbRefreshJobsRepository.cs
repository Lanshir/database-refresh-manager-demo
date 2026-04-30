using Demo.DbRefreshManager.Application.Repositories.Base;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;

namespace Demo.DbRefreshManager.Application.Repositories;

/// <summary>
/// Репозиторий задач на перезаливку БД.
/// </summary>
public interface IDbRefreshJobsRepository : IRepository<DbRefreshJob>
{
    /// <summary>
    /// Получить задачи для запуска перезаливки.
    /// </summary>
    Task<List<DbRefreshJob>> GetJobsToRun();

    /// <summary>
    /// Перевод задачи в процесс обновления.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <param name="userComment">Комментарий пользователя.</param>
    Task SetJobInProgressStatus(int jobId, string? userComment = null);

    /// <summary>
    /// Перевод задачи в статус по ум.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    Task SetJobDefaultStatus(int jobId);
}

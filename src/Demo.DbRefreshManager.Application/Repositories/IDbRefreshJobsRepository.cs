using Demo.DbRefreshManager.Application.Repositories.Base;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;

namespace Demo.DbRefreshManager.Application.Repositories;

/// <summary>
/// Репозиторий задач на перезаливку БД.
/// </summary>
public interface IDbRefreshJobsRepository : IRepository<DbRefreshJob>
{
    /// <summary>
    /// Запрос задач на перезаливку БД для отображения пользователю.
    /// </summary>
    /// <param name="id">Фильтр по id.</param>
    /// <param name="dbName">Фильтр по названию БД.</param>
    IQueryable<DbRefreshJob> GetUserDisplayJobsListQuery(int? id = null, string? dbName = null);

    /// <summary>
    /// Поиск задачи на перезаливку БД.
    /// </summary>
    /// <param name="jobId">Id задачи.</param>
    Task<DbRefreshJob?> FindJob(int jobId);

    /// <summary>
    /// Поиск задачи на перезаливку БД.
    /// </summary>
    /// <param name="dbName">азвание БД.</param>
    Task<DbRefreshJob?> FindJob(string dbName);

    /// <summary>
    /// Получить задачи для запуска перезаливки.
    /// </summary>
    Task<List<DbRefreshJob>> GetJobsToRun();

    /// <summary>
    /// Установка активности перезаливки БД по расписанию.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <param name="changedUserId">Id автора изменений.</param>
    /// <param name="isActive">Активность перезаливки.</param>
    Task SetJobScheduleActive(int jobId, int changedUserId, bool isActive);

    /// <summary>
    /// Установка пользовательского комментария  к задаче.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <param name="comment">Комментарий.</param>
    Task SetUserComment(int jobId, string? comment);

    /// <summary>
    /// Установка релизного комментария задачи на перезаливку БД.
    /// </summary>
    /// <param name="dbName">Название базы.</param>
    /// <param name="comment">Комментарий.</param>
    Task SetReleaseComment(string dbName, string? comment);

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

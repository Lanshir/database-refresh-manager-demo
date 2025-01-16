using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Dal.Repositories.Abstract.Base;

namespace Demo.DbRefreshManager.Dal.Repositories.Abstract;

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
    /// <returns>Данные изменённой задачи.</returns>
    Task<DbRefreshJob> SetJobScheduleActive(int jobId, int changedUserId, bool isActive);

    /// <summary>
    /// Запуск ручной перезаливки БД.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <param name="refreshDate">Дата начала перезаливки.</param>
    /// <param name="refreshInitiator">Инициатор перезаливки.</param>
    /// <param name="comment">Комментарий.</param>
    Task<DbRefreshJob> StartManualRefresh(int jobId, DateTime refreshDate, string refreshInitiator, string? comment);

    /// <summary>
    /// Остановка ручной перезаливки БД.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    Task<DbRefreshJob> StopManualRefresh(int jobId);

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
    /// <param name="isAppend">Добавить комментарий к предыдущему.</param>
    Task SetReleaseComment(string dbName, string? comment, bool isAppend = false);

    /// <summary>
    /// Перевод задачи в процесс обновления.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <param name="userComment">Комментарий пользователя.</param>
    Task<DbRefreshJob> SetJobInProgressStatus(int jobId, string? userComment = null);

    /// <summary>
    /// Перевод задачи в статус по ум.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    Task<DbRefreshJob> SetJobDefaultStatus(int jobId);
}

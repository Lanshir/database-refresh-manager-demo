using Demo.DbRefreshManager.Application.Repositories;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Domain.Exceptions;
using Demo.DbRefreshManager.WebApi.GraphQL.Subscriptons;
using Demo.DbRefreshManager.WebApi.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace Demo.DbRefreshManager.WebApi.GraphQL.Mutations.V1;

public class DbRefreshJobsMutationsV1
{
    /// <summary>
    /// Запуск ручной перезаливки БД.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <param name="delayMinutes">Задержка до перезаливки в минутах.</param>
    /// <param name="comment">Комментарий.</param>
    [Authorize]
    public async Task<DbRefreshJobDto> StartManualRefresh(
        ITopicEventSender eventSender,
        ILogger<DbRefreshJobsMutationsV1> logger,
        IDbRefreshJobsRepository jobsRepository,
        IDbPersonalAccessesRepository accessesRepository,
        IUserIdentityProvider userIdentity,
        int jobId, int delayMinutes, string? comment)
    {
        delayMinutes = delayMinutes > 0 ? delayMinutes : 1;

        // Поиск задачи, проверка прав.
        var job = await jobsRepository.FindJob(jobId)
            ?? throw GetJobNotFoundException(jobId);

        await ValidateJobAccessAsync(accessesRepository, userIdentity, job);

        var refreshDate = DateTime.UtcNow.AddMinutes(delayMinutes);

        await jobsRepository.StartManualRefresh(job.Id, refreshDate, userIdentity.GetUserLogin(), comment);

        var updatedJob = (await jobsRepository.FindJob(job.Id))!;
        var dto = updatedJob.ToDto();

        await SendJobChangeEventAsync(eventSender, logger, dto);

        return dto;
    }

    /// <summary>
    /// Остановка ручной перезаливки БД.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    [Authorize]
    public async Task<DbRefreshJobDto> StopManualRefresh(
        ITopicEventSender eventSender,
        ILogger<DbRefreshJobsMutationsV1> logger,
        IDbRefreshJobsRepository jobsRepository,
        IDbPersonalAccessesRepository accessesRepository,
        IUserIdentityProvider userIdentity,
        int jobId)
    {
        // Поиск задачи, проверка прав.
        var job = await jobsRepository.FindJob(jobId)
            ?? throw GetJobNotFoundException(jobId);

        await ValidateJobAccessAsync(accessesRepository, userIdentity, job);
        await jobsRepository.StopManualRefresh(jobId);

        var updatedJob = (await jobsRepository.FindJob(job.Id))!;
        var dto = updatedJob.ToDto();

        await SendJobChangeEventAsync(eventSender, logger, dto);

        return dto;
    }

    /// <summary>
    /// Установка активности перезаливки БД по расписанию.
    /// </summary>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <param name="isActive">Активность перезаливки.</param>
    [Authorize]
    public async Task<DbRefreshJobDto> SetScheduledRefreshActive(
        ITopicEventSender eventSender,
        ILogger<DbRefreshJobsMutationsV1> logger,
        IUserIdentityProvider userIdentity,
        IDbRefreshJobsRepository jobsRepository,
        IDbPersonalAccessesRepository accessesRepository,
        int jobId, bool isActive)
    {
        var changeUserId = userIdentity.GetUserId();

        // Поиск задачи, проверка прав.
        var job = await jobsRepository.FindJob(jobId)
            ?? throw GetJobNotFoundException(jobId);

        await ValidateJobAccessAsync(accessesRepository, userIdentity, job);
        await jobsRepository.SetJobScheduleActive(jobId, changeUserId, isActive);

        var updatedJob = (await jobsRepository.FindJob(job.Id))!;
        var dto = updatedJob.ToDto();

        await SendJobChangeEventAsync(eventSender, logger, dto);

        return dto;
    }

    /// <summary>
    /// Установка пользовательского комментария.
    /// </summary>
    /// <param name="jobId">Id задачи.</param>
    /// <param name="comment">Комментарий.</param>
    [Authorize]
    public async Task<DbRefreshJobDto> SetUserComment(
        ITopicEventSender eventSender,
        ILogger<DbRefreshJobsMutationsV1> logger,
        IDbRefreshJobsRepository jobsRepository,
        IDbPersonalAccessesRepository accessesRepository,
        IUserIdentityProvider userIdentity,
        int jobId, string? comment)
    {
        try
        {
            // Поиск задачи, проверка прав.
            var job = await jobsRepository.FindJob(jobId)
                ?? throw GetJobNotFoundException(jobId);

            await ValidateJobAccessAsync(accessesRepository, userIdentity, job);
            await jobsRepository.SetUserComment(jobId, comment);

            job.UserComment = comment;
            var dto = job.ToDto();

            await SendJobChangeEventAsync(eventSender, logger, dto);

            return dto;
        }
        catch (Exception exc)
        {
            throw new BusinessLogicException("Не удалось обновить пользовательский комментарий", exc);
        }
    }

    /// <summary>
    /// Установка релизного комментария задачи на перезаливку БД.
    /// </summary>
    /// <param name="dbName">Название базы.</param>
    /// <param name="comment">Комментарий.</param>
    /// <param name="isAppend">Добавить комментарий к предыдущему.</param>
    public async Task<DbRefreshJobDto> SetReleaseComment(
        ITopicEventSender eventSender,
        ILogger<DbRefreshJobsMutationsV1> logger,
        IDbRefreshJobsRepository jobsRepository,
        string dbName, string? comment, bool isAppend = false)
    {
        try
        {
            // Поиск задачи, проверка прав.
            var job = await jobsRepository.FindJob(dbName)
                ?? throw new BusinessLogicException($"Задача для БД {dbName} не найдена");

            var releaseComment = isAppend ? (job.ReleaseComment + comment).Trim('\n') : comment;

            await jobsRepository.SetReleaseComment(dbName, releaseComment);

            job.ReleaseComment = releaseComment;
            var dto = job.ToDto();

            await SendJobChangeEventAsync(eventSender, logger, dto);

            return dto;
        }
        catch (Exception exc)
        {
            throw new BusinessLogicException("Не удалось обновить комментарий релиза", exc);
        }
    }

    #region Private

    private static BusinessLogicException GetJobNotFoundException(int jobId)
        => new($"Задача с id: {jobId} не найдена");

    /// <summary>
    /// Валидация доступа пользователя к задаче перезаливки БД.
    /// </summary>
    private static async Task ValidateJobAccessAsync(
        IDbPersonalAccessesRepository accessesRepository,
        IUserIdentityProvider userIdentity,
        DbRefreshJob job)
    {
        var login = userIdentity.GetUserLogin();
        var userRoles = userIdentity.GetRoles();

        var personalAccess = await accessesRepository.UserHasAccess(login, job.Id);

        if (!personalAccess
            && !job.Group!.AccessRoles.Any(r => userRoles.Contains(r.Name.ToUpper())))
        {
            throw new BusinessLogicException(
                $"У пользователя {login} нет прав для изменения задачи id: {job.Id}");
        }
    }

    /// <summary>
    /// Отправить событие об изменении задачи на перезаливку.
    /// </summary>
    private async Task SendJobChangeEventAsync(
        ITopicEventSender eventSender,
        ILogger<DbRefreshJobsMutationsV1> logger,
        DbRefreshJobDto dto)
    {
        try
        {
            await eventSender.SendAsync(
                topicName: nameof(SubscriptionsBase.OnDbRefreshJobChange),
                message: dto);
        }
        catch (Exception exc)
        {
            logger.LogError(exc,
                "Ошибка отправки события об обновлении задачи на перезаливку БД");
        }
    }

    #endregion
}

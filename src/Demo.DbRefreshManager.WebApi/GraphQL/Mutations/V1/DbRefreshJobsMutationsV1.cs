using Demo.DbRefreshManager.Application.Features.DbRefreshing;
using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Features.UsersDbAccesses;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Repositories;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Domain.Exceptions;
using Demo.DbRefreshManager.WebApi.GraphQL.Subscriptons;
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
        IStartManualRefreshCommandHandler startManualRefresh,
        int jobId,
        int delayMinutes,
        string? comment,
        CancellationToken ct)
    {
        var result = await startManualRefresh
            .HandleAsync(new(jobId, delayMinutes, comment), ct);

        if (result.IsFailure)
            throw new BusinessLogicException(result.Error);

        var dto = result.Value!;

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
        IStopManualRefreshCommandHandler stopManualRefresh,
        int jobId,
        CancellationToken ct)
    {
        var result = await stopManualRefresh.HandleAsync(new(jobId), ct);

        if (result.IsFailure)
            throw new BusinessLogicException(result.Error);

        var dto = result.Value!;

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
        ICheckCurrentUserDbAccessQueryHandler checkUserHasAccess,
        IGetDbRefreshJobByIdQueryHandler getDbRefreshJobById,
        int jobId,
        bool isActive,
        CancellationToken ct)
    {
        var changeUserId = userIdentity.GetUserId();
        var userHasAccess = await checkUserHasAccess.HandleAsync(new(jobId), ct);

        if (userHasAccess.IsFailure)
            throw new BusinessLogicException(userHasAccess.Error);

        await jobsRepository.SetJobScheduleActive(jobId, changeUserId, isActive);

        var updatedJob = await getDbRefreshJobById.HandleAsync(jobId, ct);
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
        ICheckCurrentUserDbAccessQueryHandler checkUserHasAccess,
        IGetDbRefreshJobByIdQueryHandler getDbRefreshJobById,
        int jobId,
        string? comment,
        CancellationToken ct)
    {
        try
        {
            var userHasAccess = await checkUserHasAccess.HandleAsync(new(jobId), ct);

            if (userHasAccess.IsFailure)
                throw new BusinessLogicException(userHasAccess.Error);

            await jobsRepository.SetUserComment(jobId, comment);

            var updatedJob = await getDbRefreshJobById.HandleAsync(jobId, ct);
            var dto = updatedJob.ToDto();

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
        IFindDbRefreshJobQueryHandler findDbRefreshJob,
        string dbName,
        string? comment,
        bool isAppend = false,
        CancellationToken ct = default)
    {
        try
        {
            // Поиск задачи, проверка прав.
            var job = await findDbRefreshJob.HandleAsync(new(DbName: dbName), ct)
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
}

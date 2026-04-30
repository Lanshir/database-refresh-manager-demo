using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;
using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ManualRefresh;
using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ScheduledRefresh;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
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
        ISetScheduledRefreshAvtiveCommandHandler setScheduledRefreshActive,
        int jobId,
        bool isActive,
        CancellationToken ct)
    {
        var result = await setScheduledRefreshActive.HandleAsync(new(jobId, isActive), ct);

        if (result.IsFailure)
            throw new BusinessLogicException(result.Error);

        var dto = result.Value!;

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
        ISetDbRefreshJobUserComment setDbRefreshJobUserComment,
        int jobId,
        string? comment,
        CancellationToken ct)
    {
        var result = await setDbRefreshJobUserComment.HandleAsync(new(jobId, comment), ct);

        if (result.IsFailure)
            throw new BusinessLogicException(result.Error);

        var dto = result.Value!;

        await SendJobChangeEventAsync(eventSender, logger, dto);

        return dto;
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
        ISetDbRefreshJobReleaseCommentCommandHandler setReleaseComment,
        string dbName,
        string? comment,
        bool isAppend = false,
        CancellationToken ct = default)
    {
        var result = await setReleaseComment.HandleAsync(new(dbName, comment, isAppend), ct);

        if (result.IsFailure)
            throw new BusinessLogicException(result.Error);

        var dto = result.Value!;

        await SendJobChangeEventAsync(eventSender, logger, dto);

        return dto;
    }

    /// <summary>
    /// Отправить событие об изменении задачи на перезаливку.
    /// </summary>
    private static async Task SendJobChangeEventAsync(
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

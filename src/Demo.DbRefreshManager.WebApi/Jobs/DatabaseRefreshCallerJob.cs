using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ExecutionStatus;
using Demo.DbRefreshManager.Application.Features.Logs;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.SshService;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Core.Extensions;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.GraphQL.Subscriptons;
using HotChocolate.Subscriptions;
using Quartz;

namespace Demo.DbRefreshManager.WebApi.Jobs;

/// <summary>
/// Задача вызова перезаливок БД по расписанию.
/// </summary>
public class DatabaseRefreshCallerJob(
    IServiceProvider serviceProvider,
    ILogger<DatabaseRefreshCallerJob> logger,
    IGetReadyToRunJobsHandler getReadyToRunJobs,
    IUpdateJobToInProgressStatusHandler updateJobToInProgress,
    IUpdateJobToDefaultStatusHandler updateJobToDefaultStatus,
    ILogDbRefreshStartedHandler logDbRefreshStarted,
    ILogDbRefreshFinishedHandler logDbRefreshFinished,
    IGetDbRefreshJobByIdHandler getDbRefreshJobById,
    ITopicEventSender eventSender
    ) : IJob
{
    public async Task Execute(IJobExecutionContext ctx)
    {
        try
        {
            var jobsToRun = await getReadyToRunJobs.HandleAsync(ctx.CancellationToken);
            var tasks = new List<Task>();

            foreach (var job in jobsToRun)
            {
                var task = RunDbRefreshAsync(job);
                tasks.Add(task);
                // Запуск нескольких перезаливок приводит к ошибкам при вызове без задержки.
                await Task.Delay(200);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception exc)
        {
            logger.LogError(exc, "Ошибка выполнения задачи Quartz по перезаливке БД.");
        }
    }

    /// <summary>
    /// Запуск задачи на перезаливку.
    /// </summary>
    private async Task RunDbRefreshAsync(DbRefreshJob job)
    {
        // Log start.
        logger.LogInformation(
            "Run refresh for {dbName}\n\tat {refreshStartDate} (UTC)",
            job.DbName, $"{DateTime.UtcNow:dd.MM.yyyy HH:mm}");

        var startDate = DateTime.UtcNow;
        var isScheduled = IsScheduledRefresh(job);
        var initiator = isScheduled ? "Расписание" : job.ManualRefreshInitiator ?? "";

        try
        {
            await logDbRefreshStarted.HandleAsync(
                new(job.Id, startDate, initiator, job.SshScript),
                CancellationToken.None);
        }
        catch (Exception exc)
        {
            logger.LogError(exc, "Ошибка записи лога перезаливки БД.");
        }

        SshCommandResult? sshResult = null;
        string? error = null;

        try
        {
            // Обновление статуса задачи.
            var userComment = isScheduled ? null : job.UserComment;

            await updateJobToInProgress.HandleAsync(
                new(job.Id, userComment),
                CancellationToken.None);

            var updatedJob = await getDbRefreshJobById
                .HandleAsync(job.Id, CancellationToken.None);

            // Отправка сообщения о начале перезаливки.
            var dto = updatedJob.ToDto();

            await eventSender.SendAsync(nameof(SubscriptionsBase.OnDbRefreshJobChange), dto);

            // Вызов перезаливки БД.
            var sshClient = serviceProvider.GetRequiredService<ISshClientService>();

            // Подключение к серверу перезаливок, запуск команды.
            /*
            await sshClient.ConnectAsync("", "", "");

            sshResult = sshClient.RunCommand(job.SshScript);
            */
            // DEMO PAUSE.
            await Task.Delay(10000);
        }
        catch (Exception exc)
        {
            error = exc.Message;
            logger.LogError(exc, "Ошибка перезаливки БД {dbName}", job.DbName);
        }
        finally
        {
            // Сброс статуса на начальный.
            await updateJobToDefaultStatus
                .HandleAsync(new(job.Id), CancellationToken.None);

            var updatedJob = await getDbRefreshJobById
                .HandleAsync(job.Id, CancellationToken.None);

            // Отправка сообщения об окончании перезаливки.
            var dto = updatedJob.ToDto();

            await eventSender.SendAsync(nameof(SubscriptionsBase.OnDbRefreshJobChange), dto);

            // Log finish.
            try
            {
                await logDbRefreshFinished.HandleAsync(
                    new(job.Id, startDate, sshResult?.Code, sshResult?.Result, sshResult?.Error),
                    CancellationToken.None);
            }
            catch (Exception exc)
            {
                logger.LogError(exc, "Ошибка записи лога перезаливки БД.");
            }

            logger.LogInformation(
                "Finish refresh for {dbName}\n\tat {refreshEndDate} (UTC)",
                job.DbName, $"{DateTime.UtcNow:dd.MM.yyyy HH:mm}");
        }
    }

    /// <summary>
    /// Задача запущена по расписанию.
    /// </summary>
    private static bool IsScheduledRefresh(DbRefreshJob job)
        => job.ScheduleIsActive
            && job.ScheduleRefreshTime.UtcDateTime.TimeOfDay
                == DateTime.UtcNow.CeilToMinutes().TimeOfDay;
}

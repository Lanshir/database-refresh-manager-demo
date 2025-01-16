using Demo.DbRefreshManager.Common.Converters.Abstract;
using Demo.DbRefreshManager.Common.Extensions;
using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Demo.DbRefreshManager.Services.Abstract;
using Demo.DbRefreshManager.Services.Models.SshServiceModels;
using Demo.DbRefreshManager.WebApi.GraphQL.Subscriptons;
using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;
using HotChocolate.Subscriptions;
using Quartz;

namespace Demo.DbRefreshManager.WebApi.Jobs;

/// <summary>
/// Задача вызова перезаливок БД по расписанию.
/// </summary>
public class DatabaseRefreshCallerJob(
    IServiceProvider serviceProvider,
    ILogger<DatabaseRefreshCallerJob> logger,
    ITypeMapper mapper,
    IDbRefreshJobsRepository jobsRepository,
    IDbRefreshLogsRepository logsRepository,
    ITopicEventSender eventSender
    ) : IJob
{
    public async Task Execute(IJobExecutionContext ctx)
    {
        try
        {
            var jobsToRun = await jobsRepository.GetJobsToRun();
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
            $"Run refresh for {job.DbName}\n\tat {DateTime.UtcNow:dd.MM.yyyy HH:mm} (UTC)");

        var startDate = DateTime.UtcNow;
        var isScheduled = CheckScheduledRefresh(job);
        var initiator = isScheduled ? "Расписание" : job.ManualRefreshInitiator ?? "";

        try
        {
            await logsRepository.LogDbRefreshStart(job.Id, startDate, initiator, job.SshScript);
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

            var updatedJob = await jobsRepository.SetJobInProgressStatus(job.Id, userComment);

            // Отправка сообщения о начале перезаливки.
            var dto = mapper.Map<DbRefreshJobDto>(updatedJob);

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
            logger.LogError(exc, $"Ошибка перезаливки БД {job.DbName}");
        }
        finally
        {
            // Сброс статуса на начальный.
            var updatedJob = await jobsRepository.SetJobDefaultStatus(job.Id);

            // Отправка сообщения об окончании перезаливки.
            var dto = mapper.Map<DbRefreshJobDto>(updatedJob);

            await eventSender.SendAsync(nameof(SubscriptionsBase.OnDbRefreshJobChange), dto);

            // Log finish.
            try
            {
                await logsRepository.LogDbRefreshFinish
                    (job.Id, startDate, sshResult?.Code, sshResult?.Result, sshResult?.Error);
            }
            catch (Exception exc)
            {
                logger.LogError(exc, "Ошибка записи лога перезаливки БД.");
            }

            logger.LogInformation(
                $"Finish refresh for {job.DbName}\n\tat {DateTime.UtcNow:dd.MM.yyyy HH:mm} (UTC)");
        }
    }

    /// <summary>
    /// Задача запущена по расписанию.
    /// </summary>
    private static bool CheckScheduledRefresh(DbRefreshJob job)
        => job.ScheduleIsActive
            && job.ScheduleRefreshTime.UtcDateTime.TimeOfDay
                == DateTime.UtcNow.CeilToMinutes().TimeOfDay;
}

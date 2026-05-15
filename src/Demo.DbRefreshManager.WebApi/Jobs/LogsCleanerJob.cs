using Demo.DbRefreshManager.Application.Features.Logs;
using Quartz;

namespace Demo.DbRefreshManager.WebApi.Jobs;

/// <summary>
/// Задача очистки логов.
/// </summary>
public class LogsCleanerJob(
    ILogger<LogsCleanerJob> logger,
    IDeleteOldLogsHandler deleteOldLogs
    ) : IJob
{
    public async Task Execute(IJobExecutionContext ctx)
    {
        try
        {
            await deleteOldLogs.HandleAsync(ctx.CancellationToken);
        }
        catch (Exception exc)
        {
            logger.LogError(exc, "Ошибка задачи очистки логов.");
        }
    }
}

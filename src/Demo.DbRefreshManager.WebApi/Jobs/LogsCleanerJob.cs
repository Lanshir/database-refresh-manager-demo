using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Quartz;

namespace Demo.DbRefreshManager.WebApi.Jobs;

/// <summary>
/// Задача очистки логов.
/// </summary>
public class LogsCleanerJob(
    ILogger<LogsCleanerJob> logger,
    IDbRefreshLogsRepository logsRepository
    ) : IJob
{
    public async Task Execute(IJobExecutionContext ctx)
    {
        try
        {
            await logsRepository.CleanOld();
        }
        catch (Exception exc)
        {
            logger.LogError(exc, "Ошибка задачи очистки логов.");
        }
    }
}

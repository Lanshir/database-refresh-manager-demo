using Demo.DbRefreshManager.Application.Features.Logs;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.Logs;

internal class LogDbRefreshStartedHandler(
    IDbContextFactory<AppDbContext> contextFactory
    ): ILogDbRefreshStartedHandler
{
    public async Task<bool> HandleAsync(
        LogDbRefreshStarted.Command cmd,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        var logEntry = new DbRefreshLog
        {
            DbRefreshJobId = cmd.JobId,
            RefreshStartDate = cmd.RefreshStartDate,
            ExecutedScript = cmd.ExecutedScript,
            Initiator = cmd.Initiator
        };

        ctx.Add(logEntry);

        await ctx.SaveChangesAsync(ct);

        return true;
    }
}

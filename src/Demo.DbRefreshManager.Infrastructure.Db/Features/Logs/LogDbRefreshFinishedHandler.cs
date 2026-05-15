using Demo.DbRefreshManager.Application.Features.Logs;
using Demo.DbRefreshManager.Core.Extensions;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.Logs;

internal class LogDbRefreshFinishedHandler(
    IDbContextFactory<AppDbContext> contextFactory
    ): ILogDbRefreshFinishedHandler
{
    public async Task<bool> HandleAsync(
        LogDbRefreshFinished.Command cmd,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        await ctx.Set<DbRefreshLog>()
            .Where(l =>
                l.DbRefreshJobId == cmd.JobId
                && l.RefreshStartDate == cmd.RefreshStartDate)
            .ExecuteUpdateAsync(s => s
                .SetProperty(l => l.RefreshEndDate,
                    DateTime.UtcNow)
                .SetProperty(l => l.Code, cmd.Code)
                .SetProperty(l => l.Result, cmd.Result.TrimOrNull())
                .SetProperty(l => l.Error, cmd.Error.TrimOrNull()),
                ct);

        return true;
    }
}

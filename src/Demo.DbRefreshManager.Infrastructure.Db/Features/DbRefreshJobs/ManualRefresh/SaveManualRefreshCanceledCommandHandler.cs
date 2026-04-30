using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ManualRefresh;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs.ManualRefresh;

internal class SaveManualRefreshCanceledCommandHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : ISaveManualRefreshCanceledCommandHandler
{
    public async Task<bool> HandleAsync(
        SaveManualRefreshCanceled.Command cmd,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        await ctx.Set<DbRefreshJob>()
            .Where(j => j.Id == cmd.JobId)
            .ExecuteUpdateAsync(c =>
                c.SetProperty(j => j.ManualRefreshDate, j => null)
                .SetProperty(j => j.ManualRefreshInitiator, j => null),
                ct);

        return true;
    }
}

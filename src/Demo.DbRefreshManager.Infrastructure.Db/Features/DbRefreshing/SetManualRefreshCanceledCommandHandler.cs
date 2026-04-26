using Demo.DbRefreshManager.Application.Features.DbRefreshing;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshing;

internal class SetManualRefreshCanceledCommandHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : ISetManualRefreshCanceledCommandHandler
{
    public async Task<bool> HandleAsync(
        SetManualRefreshCanceledCommand.Dto cmd,
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

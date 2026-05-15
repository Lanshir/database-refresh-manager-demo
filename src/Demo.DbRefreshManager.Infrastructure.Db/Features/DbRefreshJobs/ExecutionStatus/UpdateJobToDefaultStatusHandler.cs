using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ExecutionStatus;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs.ExecutionStatus;

internal class UpdateJobToDefaultStatusHandler(
    IDbContextFactory<AppDbContext> contextFactory
    ) : IUpdateJobToDefaultStatusHandler
{
    public async Task<bool> HandleAsync(
        UpdateJobToDefaultStatus.Command cmd,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        await ctx.Set<DbRefreshJob>()
            .Where(j => j.Id == cmd.JobId)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(j => j.InProgress, false)
                .SetProperty(j => j.ManualRefreshDate, j => null)
                .SetProperty(j => j.ManualRefreshInitiator, j => null),
                ct);

        return true;
    }
}

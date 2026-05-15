using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ExecutionStatus;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs.ExecutionStatus;

internal class UpdateJobToInProgressStatusHandler(
    IDbContextFactory<AppDbContext> contextFactory
    ) : IUpdateJobToInProgressStatusHandler
{
    public async Task<bool> HandleAsync(
        UpdateJobToInProgressStatus.Command cmd,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        await ctx.Set<DbRefreshJob>()
            .Where(j => j.Id == cmd.JobId)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(j => j.InProgress, true)
                .SetProperty(j => j.LastRefreshDate, DateTime.UtcNow)
                .SetProperty(j => j.ReleaseComment, j => null)
                .SetProperty(j => j.UserComment, cmd.UserComment),
                ct);

        return true;
    }
}

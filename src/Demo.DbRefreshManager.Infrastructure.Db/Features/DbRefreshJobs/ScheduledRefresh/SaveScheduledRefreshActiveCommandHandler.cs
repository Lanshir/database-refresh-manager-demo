using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ScheduledRefresh;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs.ScheduledRefresh;

internal class SaveScheduledRefreshActiveCommandHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : ISaveScheduledRefreshActiveCommandHandler
{
    public async Task<bool> HandleAsync(
        SaveScheduledRefreshActive.Command cmd,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        await ctx.Set<DbRefreshJob>()
            .Where(job => job.Id == cmd.JobId)
            .ExecuteUpdateAsync(c =>
                c.SetProperty(j => j.ScheduleIsActive, cmd.IsActive)
                .SetProperty(j => j.ScheduleChangeUserId, cmd.ChangeIssuerUserId)
                .SetProperty(j => j.ScheduleChangeDate, DateTime.UtcNow),
                ct);

        return true;
    }
}

using Demo.DbRefreshManager.Application.Features.DbRefreshing;
using Demo.DbRefreshManager.Core.Extensions;
using Demo.DbRefreshManager.Core.Results;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshing;

internal class SetManualRefreshStartedCommandHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : ISetManualRefreshStartedCommandHandler
{
    public async Task<Result> HandleAsync(
        SetManualRefreshStarted.Command cmd,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        var refreshDate = cmd.RefreshDate.RoundToMinutes();
        var comment = string.IsNullOrWhiteSpace(cmd.Comment) ? null : cmd.Comment.Trim();

        await ctx.Set<DbRefreshJob>()
            .Where(job => job.Id == cmd.JobId)
            .ExecuteUpdateAsync(c =>
                c.SetProperty(j => j.ManualRefreshDate, refreshDate)
                .SetProperty(j => j.ManualRefreshInitiator, cmd.RefreshInitiator)
                .SetProperty(j => j.UserComment, comment),
                ct);

        return Result.Success();
    }
}

using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs.Comments;

internal class UpdateDbUserCommentHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : IUpdateDbUserCommentHandler
{
    public async Task<bool> HandleAsync(
        UpdateDbUserComment.Command cmd,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        await ctx.Set<DbRefreshJob>()
            .Where(job => job.Id == cmd.JobId)
            .ExecuteUpdateAsync(c =>
                c.SetProperty(j => j.UserComment, cmd.Comment),
                ct);

        return true;
    }
}

using Demo.DbRefreshManager.Application.Features.DbRefreshJobs.Comments;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs.Comments;

internal class UpdateDbRefreshJobUserCommentHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : IUpdateDbRefreshJobUserCommentHandler
{
    public async Task<bool> HandleAsync(
        UpdateDbRefreshJobUserComment.Command cmd,
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

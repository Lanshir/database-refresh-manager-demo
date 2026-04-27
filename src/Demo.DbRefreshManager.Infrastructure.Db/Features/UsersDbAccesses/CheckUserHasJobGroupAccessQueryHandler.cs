using Demo.DbRefreshManager.Application.Features.UsersDbAccesses;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.UsersDbAccesses;

internal class CheckUserHasJobGroupAccessQueryHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : ICheckUserHasJobGroupAccessQueryHandler
{
    public async Task<bool> HandleAsync(
        CheckUserHasJobGroupAccess.Query query,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        return await ctx.Set<DbRefreshJob>()
            .Where(j => j.Id == query.JobId)
            .AnyAsync(j => j.Group!.AccessRoles.Any(r => query.UserRoles.Contains(r.Name)), ct);
    }
}

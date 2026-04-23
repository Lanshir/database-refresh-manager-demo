using Demo.DbRefreshManager.Application.Features.UsersDbAccesses;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.UsersDbAccesses;

internal class UserHasJobGroupAccessQueryHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : IUserHasJobGroupAccessQueryHandler
{
    public async Task<bool> HandleAsync(
        UserHasJobGroupAccessQuery.Dto query,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        return await ctx.Set<DbRefreshJob>()
            .Where(j => j.Id == query.JobId)
            .AnyAsync(j => j.Group!.AccessRoles.Any(r => query.UserRoles.Contains(r.Name)), ct);
    }
}

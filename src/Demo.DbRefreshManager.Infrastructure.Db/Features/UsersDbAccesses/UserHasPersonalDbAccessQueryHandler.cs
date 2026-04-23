using Demo.DbRefreshManager.Application.Features.UsersDbAccesses;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbUsersAccesses;

internal class UserHasPersonalDbAccessQueryHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : IUserHasPersonalDbAccessQueryHandler
{
    public async Task<bool> HandleAsync(
        UserHasPersonalDbAccessQuery.Dto query,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        return await ctx.Set<DbPersonalAccess>()
            .Where(a => a.Login.ToUpper() == query.Login.ToUpper()
                && a.JobId == query.JobId)
            .Select(a => true)
            .FirstOrDefaultAsync(ct);
    }
}

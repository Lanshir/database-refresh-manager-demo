using Demo.DbRefreshManager.Application.Features.UsersDbAccesses;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.UsersDbAccesses;

internal class CheckUserHasPersonalDbAccessQueryHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : ICheckUserHasPersonalDbAccessQueryHandler
{
    public async Task<bool> HandleAsync(
        CheckUserHasPersonalDbAccess.Query query,
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

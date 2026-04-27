using Demo.DbRefreshManager.Application.Features.UsersDbAccesses;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.UsersDbAccesses;

internal class GetPersonalAccessJobIdsQueryHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : IGetPersonalAccessJobIdsQueryHandler
{
    public async Task<int[]> HandleAsync(
        GetPersonalAccessJobIds.Query query,
        CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        return await ctx.Set<DbPersonalAccess>()
            .Where(a => a.Login.ToUpper() == query.Login.ToUpper())
            .Select(a => a.JobId)
            .ToArrayAsync(ct);
    }
}

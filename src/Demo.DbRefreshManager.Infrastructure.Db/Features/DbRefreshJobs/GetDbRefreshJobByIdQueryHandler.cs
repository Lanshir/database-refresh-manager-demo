using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs;

internal class GetDbRefreshJobByIdQueryHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : IGetDbRefreshJobByIdQueryHandler
{
    public async Task<DbRefreshJob> HandleAsync(int jobId, CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        return await ctx.Set<DbRefreshJob>()
            .Include(j => j.ScheduleChangeUser)
            .Include(j => j.Group)
            .ThenInclude(g => g!.AccessRoles)
            .FirstAsync(j => j.Id == jobId, ct);
    }
}

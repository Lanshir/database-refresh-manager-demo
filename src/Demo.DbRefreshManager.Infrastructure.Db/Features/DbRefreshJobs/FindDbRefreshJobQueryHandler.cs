using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs;

internal class FindDbRefreshJobQueryHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : IFindDbRefreshJobQueryHandler
{
    public async Task<DbRefreshJob?> HandleAsync(
        FindDbRefreshJob.Query query,
        CancellationToken ct)
    {
        var (JobId, DbName) = query;
        using var ctx = contextFactory.CreateDbContext();

        return await ctx.Set<DbRefreshJob>()
            .Include(j => j.ScheduleChangeUser)
            .Include(j => j.Group)
            .ThenInclude(g => g!.AccessRoles)
            .FirstOrDefaultAsync(j =>
                (JobId != null || DbName != null)
                && JobId != null && j.Id == JobId
                && DbName != null && j.DbName.ToUpper() == DbName.ToUpper(),
                ct);
    }
}

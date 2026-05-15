using Demo.DbRefreshManager.Application.Features.Logs;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.Logs;

internal class GetLogsForDisplayHandler(
    IDbContextFactory<AppDbContext> contextFactory
    ): IGetLogsForDisplayHandler, IDisposable
{
    private readonly AppDbContext _ctx = contextFactory.CreateDbContext();

    public IQueryable<DbRefreshLog> Handle(GetLogsForDisplay.Query query)
    {
        var (jobId, startDate) = query;

        return _ctx.Set<DbRefreshLog>()
            .Where(l => !l.DbRefreshJob!.IsDeleted
                && (jobId == null || l.DbRefreshJobId == jobId)
                && (startDate == null
                    || l.RefreshStartDate >= startDate && l.RefreshStartDate < startDate.Value.AddDays(1)))
            .OrderByDescending(l => l.RefreshStartDate)
            .ThenBy(l => l.DbRefreshJob!.Group!.Id)
            .ThenBy(l => l.DbRefreshJob!.Id)
            .Take(2000);
    }

    public void Dispose() => _ctx.Dispose();
}

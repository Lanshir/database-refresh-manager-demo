using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs;

internal class GetDbRefreshJobsForDisplayQueryHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : IGetDbRefreshJobsForDisplayQueryHandler, IDisposable
{
    private readonly AppDbContext _ctx = contextFactory.CreateDbContext();

    public IQueryable<DbRefreshJobDto> Handle(GetDbRefreshJobsForDisplay.Query query)
    {
        var (Id, DbName) = query;

        return _ctx.Set<DbRefreshJob>()
            .Where(j => !j.IsDeleted && j.Group!.IsVisible
                && (Id == null || j.Id == Id)
                && (DbName == null || j.DbName.ToUpper() == DbName.ToUpper()))
            .OrderBy(j => j.Group!.SortOrder)
            .ThenBy(j => j.Id)
            .Select(DbRefreshJob.ToDtoProjectionExpression);
    }

    public void Dispose() => _ctx.Dispose();
}

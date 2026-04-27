using Demo.DbRefreshManager.Application.Features.DbGroups;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbGroups;

internal class GetUserDisplayGroupsQueryHandler(
    IDbContextFactory<AppDbContext> contextFactory)
    : IGetUserDisplayGroupsQueryHandler, IDisposable
{
    private readonly AppDbContext _ctx = contextFactory.CreateDbContext();

    public IQueryable<DbGroupDto> Handle()
        => _ctx.Set<DbGroup>()
            .Where(g => g.IsVisible)
            .OrderBy(g => g.SortOrder)
            .Select(DbGroup.ToDtoProjectionExpression);

    public void Dispose() => _ctx.Dispose();
}

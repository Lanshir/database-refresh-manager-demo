using Demo.DbRefreshManager.Dal.Context;
using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Demo.DbRefreshManager.Dal.Repositories.Concrete.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Dal.Repositories.Concrete;

/// <inheritdoc cref="IDbGroupsRepository" />
internal class DbGroupsRepository(
    IDbContextFactory<AppDbContext> contextFactory
    ) : BaseRepository<DbGroup>(contextFactory), IDbGroupsRepository
{
    public IQueryable<DbGroup> GetUserDisplayGroupsQuery() => Get()
        .Where(g => g.IsVisible)
        .OrderBy(g => g.SortOrder);
}

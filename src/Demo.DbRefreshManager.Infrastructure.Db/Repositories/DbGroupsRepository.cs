using Demo.DbRefreshManager.Application.Repositories;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Demo.DbRefreshManager.Infrastructure.Db.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Repositories;

/// <inheritdoc cref="IDbGroupsRepository" />
internal class DbGroupsRepository(
    IDbContextFactory<AppDbContext> contextFactory
    ) : BaseRepository<DbGroup>(contextFactory), IDbGroupsRepository
{
    public IQueryable<DbGroup> GetUserDisplayGroupsQuery() => GetQueriable()
        .Where(g => g.IsVisible)
        .OrderBy(g => g.SortOrder);
}

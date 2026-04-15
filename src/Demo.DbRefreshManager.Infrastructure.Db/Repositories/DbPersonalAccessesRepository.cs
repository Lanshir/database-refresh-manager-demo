using Demo.DbRefreshManager.Application.Repositories;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Demo.DbRefreshManager.Infrastructure.Db.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Repositories;

/// <inheritdoc cref="IDbPersonalAccessesRepository" />
internal class DbPersonalAccessesRepository(
    IDbContextFactory<AppDbContext> contextFactory
    ) : BaseRepository<DbPersonalAccess>(contextFactory), IDbPersonalAccessesRepository
{
    public async Task<int[]> GetPersonalAccessJobIds(string login)
        => await GetQueriable()
            .Where(a => a.Login.ToUpper() == login.ToUpper())
            .Select(a => a.JobId)
            .ToArrayAsync();

    public async Task<bool> UserHasAccess(string login, int jobId)
        => await GetQueriable()
            .Where(a => a.Login.ToUpper() == login.ToUpper()
                && a.JobId == jobId)
            .Select(a => true)
            .FirstOrDefaultAsync();
}

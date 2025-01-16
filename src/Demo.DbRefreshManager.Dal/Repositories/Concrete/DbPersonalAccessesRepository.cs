using Demo.DbRefreshManager.Dal.Context;
using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Demo.DbRefreshManager.Dal.Repositories.Concrete.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Dal.Repositories.Concrete;

/// <inheritdoc cref="IDbPersonalAccessesRepository" />
internal class DbPersonalAccessesRepository(
    IDbContextFactory<AppDbContext> contextFactory
    ) : BaseRepository<DbPersonalAccess>(contextFactory), IDbPersonalAccessesRepository
{
    public async Task<int[]> GetPersonalAccessJobIds(string login)
        => await Get()
            .Where(a => a.Login.ToUpper() == login.ToUpper())
            .Select(a => a.JobId)
            .ToArrayAsync();

    public async Task<bool> UserHasAccess(string login, int jobId)
        => await Get()
            .Where(a => a.Login.ToUpper() == login.ToUpper()
                && a.JobId == jobId)
            .Select(a => true)
            .FirstOrDefaultAsync();
}

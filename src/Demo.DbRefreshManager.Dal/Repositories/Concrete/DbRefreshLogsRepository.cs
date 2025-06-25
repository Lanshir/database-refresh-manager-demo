using Demo.DbRefreshManager.Common.Extensions;
using Demo.DbRefreshManager.Dal.Context;
using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Demo.DbRefreshManager.Dal.Repositories.Concrete.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Dal.Repositories.Concrete;

/// <inheritdoc cref="IDbRefreshLogsRepository" />
internal class DbRefreshLogsRepository(
    IDbContextFactory<AppDbContext> contextFactory
    ) : BaseRepository<DbRefreshLog>(contextFactory), IDbRefreshLogsRepository
{
    public IQueryable<DbRefreshLog> GetUserDisplayLogsQuery(int? jobId = null, DateTime? startDate = null)
        => GetQueriable()
            .Where(l => !l.DbRefreshJob!.IsDeleted
                && (jobId == null || l.DbRefreshJobId == jobId)
                && (startDate == null
                    || l.RefreshStartDate >= startDate && l.RefreshStartDate < startDate.Value.AddDays(1)))
            .OrderByDescending(l => l.RefreshStartDate)
            .ThenBy(l => l.DbRefreshJob!.Group!.Id)
            .ThenBy(l => l.DbRefreshJob!.Id)
            .Take(2000);

    public async Task LogDbRefreshStart(int jobId, DateTime refreshStartDate, string initiator, string executedScript)
    {
        var logEntry = new DbRefreshLog
        {
            DbRefreshJobId = jobId,
            RefreshStartDate = refreshStartDate,
            ExecutedScript = executedScript,
            Initiator = initiator
        };

        await CreateAsync(logEntry);
    }

    public async Task LogDbRefreshFinish
        (int jobId, DateTime refreshStartDate, int? code, string? result, string? error)
        => await UpdatePropsAsync(s => s
            .SetProperty(l => l.RefreshEndDate,
                DateTime.UtcNow)
            .SetProperty(l => l.Code, code)
            .SetProperty(l => l.Result, result.TrimOrNull())
            .SetProperty(l => l.Error, error.TrimOrNull()),
            where: l => l.DbRefreshJobId == jobId
                && l.RefreshStartDate == refreshStartDate);

    public async Task CleanOld() => await DeleteAsync(
        where: l => l.RefreshStartDate < DateTime.UtcNow.AddMonths(-3));
}

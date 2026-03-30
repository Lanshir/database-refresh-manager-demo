using Demo.DbRefreshManager.Common.Extensions;
using Demo.DbRefreshManager.Dal.Context;
using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Demo.DbRefreshManager.Dal.Repositories.Concrete.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Dal.Repositories.Concrete;

/// <inheritdoc cref="IDbRefreshJobsRepository" />
internal class DbRefreshJobsRepository(
    IDbContextFactory<AppDbContext> contextFactory
    ) : BaseRepository<DbRefreshJob>(contextFactory), IDbRefreshJobsRepository, IDisposable
{
    public IQueryable<DbRefreshJob> GetUserDisplayJobsListQuery(int? id = null, string? dbName = null)
        => GetQueriable()
            .Where(j => !j.IsDeleted && j.Group!.IsVisible
                && (id == null || j.Id == id)
                && (dbName == null || j.DbName.ToUpper() == dbName.ToUpper()))
            .OrderBy(j => j.Group!.SortOrder)
            .ThenBy(j => j.Id);

    public async Task<DbRefreshJob?> FindJob(int jobId)
        => await GetQueriable()
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .Include(j => j.ScheduleChangeUser)
            .SingleOrDefaultAsync(j => j.Id == jobId);

    public async Task<DbRefreshJob?> FindJob(string dbName)
        => await GetQueriable()
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .Include(j => j.ScheduleChangeUser)
            .SingleOrDefaultAsync(j => j.DbName.ToUpper() == dbName.ToUpper());

    public async Task<List<DbRefreshJob>> GetJobsToRun()
    {
        var nowDateTime = DateTime.UtcNow.CeilToMinutes();

        // Получение всех возможных задач для запуска.
        var jobsToRun = await GetQueriable()
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .Include(j => j.ScheduleChangeUser)
            .Where(j => !j.IsDeleted && !j.InProgress)
            .ToListAsync();

        jobsToRun = [..jobsToRun
            // Фильтр по условию ручного запуска.
            .Where(j =>
                // Фильтр по условию ручного запуска.
                (j.ManualRefreshDate != null && j.ManualRefreshDate <= nowDateTime)
                ||
                // Фильтр по условию запуска по расписанию.
                (j.ScheduleIsActive && j.ScheduleRefreshTime.UtcDateTime.TimeOfDay == nowDateTime.TimeOfDay))
            ];

        return jobsToRun;
    }

    public async Task SetJobScheduleActive(int jobId, int changedUserId, bool isActive)
        => await UpdatePropsAsync(c =>
            c.SetProperty(j => j.ScheduleIsActive, isActive)
            .SetProperty(j => j.ScheduleChangeUserId, changedUserId)
            .SetProperty(j => j.ScheduleChangeDate, DateTime.UtcNow),
            where: job => job.Id == jobId);

    public async Task StartManualRefresh(int jobId, DateTime refreshDate, string refreshInitiator, string? comment)
    {
        refreshDate = refreshDate.RoundToMinutes();
        comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();

        await UpdatePropsAsync(c =>
            c.SetProperty(j => j.ManualRefreshDate, refreshDate)
            .SetProperty(j => j.ManualRefreshInitiator, refreshInitiator)
            .SetProperty(j => j.UserComment, comment),
                where: job => job.Id == jobId);
    }

    public async Task StopManualRefresh(int jobId)
        => await UpdatePropsAsync(c =>
            c.SetProperty(j => j.ManualRefreshDate, j => null)
            .SetProperty(j => j.ManualRefreshInitiator, j => null),
            where: job => job.Id == jobId);

    public async Task SetUserComment(int jobId, string? comment)
        => await UpdatePropsAsync(c =>
            c.SetProperty(j => j.UserComment, comment),
            where: job => job.Id == jobId);

    public async Task SetReleaseComment(string dbName, string? comment)
        => await UpdatePropsAsync(c =>
            c.SetProperty(j => j.ReleaseComment, j => comment),
            where: job => job.DbName.ToUpper() == dbName.ToUpper());

    public async Task SetJobInProgressStatus(int jobId, string? userComment = null)
        => await UpdatePropsAsync(s =>
            s.SetProperty(j => j.InProgress, true)
            .SetProperty(j => j.LastRefreshDate, DateTime.UtcNow)
            .SetProperty(j => j.ReleaseComment, j => null)
            .SetProperty(j => j.UserComment, userComment),
            where: j => j.Id == jobId);

    public async Task SetJobDefaultStatus(int jobId)
        => await UpdatePropsAsync(s =>
            s.SetProperty(j => j.InProgress, false)
            .SetProperty(j => j.ManualRefreshDate, j => null)
            .SetProperty(j => j.ManualRefreshInitiator, j => null),
            where: j => j.Id == jobId);
}

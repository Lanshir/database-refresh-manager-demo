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
    ) : BaseRepository<DbRefreshJob>(contextFactory), IDbRefreshJobsRepository
{
    public IQueryable<DbPersonalAccess> GetPersonalAccesses(string login)
    {
        var ctx = ContextFactory.CreateDbContext();

        return ctx.Set<DbPersonalAccess>()
            .Where(a => a.Login.ToUpper() == login.ToUpper());
    }

    public IQueryable<DbRefreshJob> GetUserDisplayJobsListQuery(int? id = null, string? dbName = null)
        => Get()
            .Where(j => !j.IsDeleted && j.Group!.IsVisible
                && (id == null || j.Id == id)
                && (dbName == null || j.DbName.ToUpper() == dbName.ToUpper()))
            .OrderBy(j => j.Group!.SortOrder)
            .ThenBy(j => j.Id);

    public async Task<DbRefreshJob?> FindJob(int jobId)
        => await Get()
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .Include(j => j.ScheduleChangeUser)
            .SingleOrDefaultAsync(j => j.Id == jobId);

    public async Task<DbRefreshJob?> FindJob(string dbName)
        => await Get()
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .Include(j => j.ScheduleChangeUser)
            .SingleOrDefaultAsync(j => j.DbName.ToUpper() == dbName.ToUpper());

    public async Task<List<DbRefreshJob>> GetJobsToRun()
    {
        var nowDateTime = DateTime.UtcNow.CeilToMinutes();

        // Получение всех возможных задач для запуска.
        var jobsToRun = await Get()
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .Include(j => j.ScheduleChangeUser)
            .Where(j => !j.IsDeleted && !j.InProgress)
            .ToListAsync();

        jobsToRun = jobsToRun
            // Фильтр по условию ручного запуска.
            .Where(j =>
                // Фильтр по условию ручного запуска.
                (j.ManualRefreshDate != null && j.ManualRefreshDate <= nowDateTime)
                ||
                // Фильтр по условию запуска по расписанию.
                (j.ScheduleIsActive && j.ScheduleRefreshTime.UtcDateTime.TimeOfDay == nowDateTime.TimeOfDay))
            .ToList();

        return jobsToRun;
    }

    public async Task<DbRefreshJob> SetJobScheduleActive(int jobId, int changedUserId, bool isActive)
    {
        var resultQuery = await UpdatePropsReturningAsync(c =>
            c.SetProperty(j => j.ScheduleIsActive, isActive)
            .SetProperty(j => j.ScheduleChangeUserId, changedUserId)
            .SetProperty(j => j.ScheduleChangeDate, DateTime.UtcNow),
            where: job => job.Id == jobId);

        return await resultQuery
            .Include(j => j.ScheduleChangeUser)
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .SingleAsync();
    }

    public async Task<DbRefreshJob> StartManualRefresh(int jobId, DateTime refreshDate, string refreshInitiator, string? comment)
    {
        refreshDate = refreshDate.RoundToMinutes();
        comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();

        var resultQuery = await UpdatePropsReturningAsync(c =>
            c.SetProperty(j => j.ManualRefreshDate, refreshDate)
            .SetProperty(j => j.ManualRefreshInitiator, refreshInitiator)
            .SetProperty(j => j.UserComment, comment),
                where: job => job.Id == jobId);

        return await resultQuery
            .Include(j => j.ScheduleChangeUser)
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .SingleAsync();
    }

    public async Task<DbRefreshJob> StopManualRefresh(int jobId)
    {
        var resultQuery = await UpdatePropsReturningAsync(c =>
            c.SetProperty(j => j.ManualRefreshDate, j => null)
            .SetProperty(j => j.ManualRefreshInitiator, j => null),
            where: job => job.Id == jobId);

        return await resultQuery
            .Include(j => j.ScheduleChangeUser)
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .SingleAsync();
    }

    public async Task SetUserComment(int jobId, string? comment)
        => await UpdatePropsAsync(c =>
            c.SetProperty(j => j.UserComment, comment),
            where: job => job.Id == jobId);

    public async Task SetReleaseComment(string dbName, string? comment, bool isAppend = false)
        => await UpdatePropsAsync(c =>
            c.SetProperty(
                j => j.ReleaseComment,
                j => isAppend ? (j.ReleaseComment + comment).Trim('\n') : comment),
            where: job => job.DbName.ToUpper() == dbName.ToUpper());

    public async Task<DbRefreshJob> SetJobInProgressStatus(int jobId, string? userComment = null)
    {
        var resultQuery = await UpdatePropsReturningAsync(s =>
            s.SetProperty(j => j.InProgress, true)
            .SetProperty(j => j.LastRefreshDate, DateTime.UtcNow)
            .SetProperty(j => j.ReleaseComment, j => null)
            .SetProperty(j => j.UserComment, userComment),
            where: j => j.Id == jobId);

        return await resultQuery
            .Include(j => j.ScheduleChangeUser)
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .SingleAsync();
    }

    public async Task<DbRefreshJob> SetJobDefaultStatus(int jobId)
    {
        var resultQuery = await UpdatePropsReturningAsync(s =>
            s.SetProperty(j => j.InProgress, false)
            .SetProperty(j => j.ManualRefreshDate, j => null)
            .SetProperty(j => j.ManualRefreshInitiator, j => null),
            where: j => j.Id == jobId);

        return await resultQuery
            .Include(j => j.ScheduleChangeUser)
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .SingleAsync();
    }
}

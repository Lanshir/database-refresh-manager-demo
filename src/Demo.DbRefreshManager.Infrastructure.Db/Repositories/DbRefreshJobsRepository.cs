using Demo.DbRefreshManager.Application.Repositories;
using Demo.DbRefreshManager.Core.Extensions;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Demo.DbRefreshManager.Infrastructure.Db.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Repositories;

/// <inheritdoc cref="IDbRefreshJobsRepository" />
internal class DbRefreshJobsRepository(
    IDbContextFactory<AppDbContext> contextFactory
    ) : BaseRepository<DbRefreshJob>(contextFactory), IDbRefreshJobsRepository, IDisposable
{
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

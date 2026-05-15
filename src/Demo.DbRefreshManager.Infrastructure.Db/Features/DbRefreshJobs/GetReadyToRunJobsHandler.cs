using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Core.Extensions;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.DbRefreshJobs;

internal class GetReadyToRunJobsHandler(
    IDbContextFactory<AppDbContext> contextFactory
    ) : IGetReadyToRunJobsHandler
{
    public async Task<List<DbRefreshJob>> HandleAsync(CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        // Получение всех возможных задач для запуска.
        var jobsToRun = await ctx.Set<DbRefreshJob>()
            .Include(j => j.Group)
            .Include(j => j.Group!.AccessRoles)
            .Include(j => j.ScheduleChangeUser)
            .Where(j => !j.IsDeleted && !j.InProgress)
            .ToListAsync(ct);

        var nowDateTime = DateTime.UtcNow.CeilToMinutes();

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
}

using Demo.DbRefreshManager.Application.Features.Logs;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.Logs;

internal class DeleteOldLogsHandler(
    IDbContextFactory<AppDbContext> contextFactory
    ): IDeleteOldLogsHandler
{
    public async Task<bool> HandleAsync(CancellationToken ct)
    {
        using var ctx = contextFactory.CreateDbContext();

        await ctx.Set<DbRefreshLog>()
            .Where(l => l.RefreshStartDate < DateTime.UtcNow.AddMonths(-3))
            .ExecuteDeleteAsync(ct);

        return true;
    }
}

using Demo.DbRefreshManager.Application.Features.Healthchecks;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.Healthchecks;

internal class EfCoreHealthcheckCommandHandler(
    IDbContextFactory<AppDbContext> ctxFactory
    ) : IEfCoreHealthcheckCommandHandler
{
    public async Task HandleAsync(CancellationToken ct)
    {
        using var ctx = await ctxFactory.CreateDbContextAsync(ct);

        await ctx.Database.ExecuteSqlRawAsync("select 1", ct);
    }
}

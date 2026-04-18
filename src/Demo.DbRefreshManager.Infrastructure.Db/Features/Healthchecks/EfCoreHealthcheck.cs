using Demo.DbRefreshManager.Application.Features.Healthchecks;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Demo.DbRefreshManager.Infrastructure.Db.Features.Healthchecks;

internal static class EfCoreHealthcheck
{
    internal class Handler(IDbContextFactory<AppDbContext> ctxFactory) : IEfCoreHealthcheckHandler
    {
        public async Task HandleAsync(CancellationToken ct)
        {
            var ctx = await ctxFactory.CreateDbContextAsync(ct);

            await ctx.Database.ExecuteSqlRawAsync("select 1", ct);
        }
    }
}

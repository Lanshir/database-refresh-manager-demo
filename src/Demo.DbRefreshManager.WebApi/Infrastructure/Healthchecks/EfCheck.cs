using Demo.DbRefreshManager.Dal.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Healthchecks;

/// <summary>
/// Проверка подключения EF Core.
/// </summary>
public class EfCheck(IDbContextFactory<AppDbContext> ctxFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var ctx = await ctxFactory.CreateDbContextAsync(cancellationToken);

            await ctx.Database.ExecuteSqlRawAsync("select 1", cancellationToken);

            return HealthCheckResult.Healthy(
                "Entity Framework request ok");
        }
        catch (Exception exc)
        {
            return HealthCheckResult.Unhealthy(
                "Entity Framework request fail", exc);
        }
    }
}

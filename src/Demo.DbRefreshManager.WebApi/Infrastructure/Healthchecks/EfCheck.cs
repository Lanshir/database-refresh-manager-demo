using Demo.DbRefreshManager.Application.Features.Healthchecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Healthchecks;

/// <summary>
/// Проверка подключения EF Core.
/// </summary>
public class EfCheck(IEfCoreHealthcheckHandler efHealthcheck) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken ct)
    {
        try
        {
            await efHealthcheck.HandleAsync(ct);

            return HealthCheckResult.Healthy("Entity Framework request ok");
        }
        catch (Exception exc)
        {
            return HealthCheckResult.Unhealthy("Entity Framework request fail", exc);
        }
    }
}

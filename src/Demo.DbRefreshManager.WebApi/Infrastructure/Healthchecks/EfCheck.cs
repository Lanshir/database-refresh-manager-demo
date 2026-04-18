using Demo.DbRefreshManager.Application.Features.Healthchecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Healthchecks;

/// <summary>
/// Проверка подключения EF Core.
/// </summary>
public class EfCheck(IEfCoreHealthcheckCommandHandler efHealthcheckCmd) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken ct)
    {
        try
        {
            await efHealthcheckCmd.HandleAsync(ct);

            return HealthCheckResult.Healthy("Entity Framework request ok");
        }
        catch (Exception exc)
        {
            return HealthCheckResult.Unhealthy("Entity Framework request fail", exc);
        }
    }
}

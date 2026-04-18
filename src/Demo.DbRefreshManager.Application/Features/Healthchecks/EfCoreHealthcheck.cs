namespace Demo.DbRefreshManager.Application.Features.Healthchecks;

/// <summary>
/// Команда проверки работоспособности EfCore.
/// </summary>
public interface IEfCoreHealthcheckHandler
{
    Task HandleAsync(CancellationToken ct);
}

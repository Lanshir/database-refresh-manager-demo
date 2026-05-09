using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ManualRefresh;

/// <summary>
/// Запись отмены ручной перезаливки.
/// </summary>
public interface IUpdateDbManualRefreshCanceledHandler
    : IAsyncHandler<bool, UpdateDbManualRefreshCanceled.Command>;

public static class UpdateDbManualRefreshCanceled
{
    public record struct Command(int JobId);
}

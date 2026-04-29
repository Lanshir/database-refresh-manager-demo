using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshing;

/// <summary>
/// Запись отмены ручной перезаливки.
/// </summary>
public interface ISetManualRefreshCanceledCommandHandler
    : IAsyncHandler<bool, SetManualRefreshCanceled.Command>;

public static class SetManualRefreshCanceled
{
    public record struct Command(int JobId);
}

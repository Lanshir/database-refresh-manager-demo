using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshing;

/// <summary>
/// Запись отмены ручной перезаливки.
/// </summary>
public interface ISetManualRefreshCanceledCommandHandler
    : IAsyncHandler<Result, SetManualRefreshCanceled.Command>;

public static class SetManualRefreshCanceled
{
    public record struct Command(int JobId);
}

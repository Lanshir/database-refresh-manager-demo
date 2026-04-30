using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ManualRefresh;

/// <summary>
/// Запись отмены ручной перезаливки.
/// </summary>
public interface ISaveManualRefreshCanceledCommandHandler
    : IAsyncHandler<bool, SaveManualRefreshCanceled.Command>;

public static class SaveManualRefreshCanceled
{
    public record struct Command(int JobId);
}

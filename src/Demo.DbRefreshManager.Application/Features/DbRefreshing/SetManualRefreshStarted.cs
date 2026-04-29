using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshing;

/// <summary>
/// Запись начала ручной перезаливки.
/// </summary>
public interface ISetManualRefreshStartedCommandHandler
    : IAsyncHandler<bool, SetManualRefreshStarted.Command>;

public class SetManualRefreshStarted
{
    public record struct Command(
        int JobId,
        DateTime RefreshDate,
        string RefreshInitiator,
        string? Comment);
}

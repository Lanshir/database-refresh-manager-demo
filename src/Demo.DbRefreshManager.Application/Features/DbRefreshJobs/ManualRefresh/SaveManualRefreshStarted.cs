using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs.ManualRefresh;

/// <summary>
/// Запись начала ручной перезаливки.
/// </summary>
public interface ISaveManualRefreshStartedCommandHandler
    : IAsyncHandler<bool, SaveManualRefreshStarted.Command>;

public class SaveManualRefreshStarted
{
    public record struct Command(
        int JobId,
        DateTime RefreshDate,
        string RefreshInitiator,
        string? Comment);
}

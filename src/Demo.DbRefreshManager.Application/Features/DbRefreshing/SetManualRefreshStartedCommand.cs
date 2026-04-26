using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshing;

/// <summary>
/// Команда записи начала ручной перезаливки.
/// </summary>
public interface ISetManualRefreshStartedCommandHandler
    : IAsyncHandler<bool, SetManualRefreshStartedCommand.Dto>;

public class SetManualRefreshStartedCommand
{
    public record struct Dto(
        int JobId,
        DateTime RefreshDate,
        string RefreshInitiator,
        string? Comment);
}

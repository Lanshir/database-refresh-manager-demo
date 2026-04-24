using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Application.Features.DbRefresh;

/// <summary>
/// Команда записи начала ручной перезаливки.
/// </summary>
public interface ISetManualRefreshStartedCommandHandler
    : IAsyncHandler<Result, SetManualRefreshStartedCommand.Dto>;

public class SetManualRefreshStartedCommand
{
    public record struct Dto(
        int JobId,
        DateTime RefreshDate,
        string RefreshInitiator,
        string? Comment);
}

using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshing;

/// <summary>
/// Команда записи отмены ручной перезаливки.
/// </summary>
public interface ISetManualRefreshCanceledCommandHandler
    : IAsyncHandler<bool, SetManualRefreshCanceledCommand.Dto>;

public static class SetManualRefreshCanceledCommand
{
    public record struct Dto(int JobId);
}

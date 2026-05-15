using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.Logs;

/// <summary>
/// Очистка устаревших логов.
/// </summary>
public interface IDeleteOldLogsHandler : IAsyncHandler<bool>;
using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.Healthchecks;

/// <summary>
/// Проверка работоспособности EfCore.
/// </summary>
public interface IEfCoreHealthcheckCommandHandler : IAsyncHandler;
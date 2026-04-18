using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.Healthchecks;

/// <summary>
/// Команда проверки работоспособности EfCore.
/// </summary>
public interface IEfCoreHealthcheckCommandHandler : IAsyncHandler;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs;

/// <summary>
/// Получить задачи готовые к запуску.
/// </summary>
public interface IGetReadyToRunJobsHandler : IAsyncHandler<List<DbRefreshJob>>;

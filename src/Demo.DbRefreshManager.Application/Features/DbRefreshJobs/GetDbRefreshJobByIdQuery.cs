using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs;

/// <summary>
/// Зарпос данных задачи на перезаливку по id.
/// </summary>
public interface IGetDbRefreshJobByIdQueryHandler : IAsyncHandler<DbRefreshJob, int>;

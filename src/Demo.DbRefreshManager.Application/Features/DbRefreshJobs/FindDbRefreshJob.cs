using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs;

/// <summary>
/// Поиск задачи на перезаливку БД.
/// </summary>
public interface IFindDbRefreshJobQueryHandler
    : IAsyncHandler<DbRefreshJob?, FindDbRefreshJob.Query>;

public static class FindDbRefreshJob
{
    /// <param name="JobId">Id задачи.</param>
    /// <param name="DbName">Название БД.</param>
    public record struct Query(int? JobId = null, string? DbName = null);
}

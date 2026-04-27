using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbRefreshJobs;

/// <summary>
/// Запрос задач на перезаливку БД для отображения пользователю.
/// </summary>
public interface IGetDbRefreshJobsForDisplayQueryHandler
    : IHandler<IQueryable<DbRefreshJobDto>, GetDbRefreshJobsForDisplay.Query>;

public static class GetDbRefreshJobsForDisplay
{
    public record struct Query(int? Id = null, string? DbName = null);
}

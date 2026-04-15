using Demo.DbRefreshManager.Application.Repositories.Base;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;

namespace Demo.DbRefreshManager.Application.Repositories;

/// <summary>
/// Репозиторий групп БД.
/// </summary>
public interface IDbGroupsRepository : IRepository<DbGroup>
{
    /// <summary>
    /// Запрос групп БД для отображения пользователю.
    /// </summary>
    IQueryable<DbGroup> GetUserDisplayGroupsQuery();
}

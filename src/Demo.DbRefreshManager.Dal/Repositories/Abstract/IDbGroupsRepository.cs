using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Dal.Repositories.Abstract.Base;

namespace Demo.DbRefreshManager.Dal.Repositories.Abstract;

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

using Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.Dal.Repositories.Abstract.Base;

namespace Demo.DbRefreshManager.Dal.Repositories.Abstract;

/// <summary>
/// Репозиторий персональных доступов к БД.
/// </summary>
public interface IDbPersonalAccessesRepository : IRepository<DbPersonalAccess>
{
    /// <summary>
    /// Запрос id задач с персональным доступом для пользователя.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    Task<int[]> GetPersonalAccessJobIds(string login);

    /// <summary>
    /// Проверка наличия доступа к БД у пользователя.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="jobId">Id задачи на перезаливку.</param>
    /// <returns>Наличие доступа.</returns>
    Task<bool> UserHasAccess(string login, int jobId);
}

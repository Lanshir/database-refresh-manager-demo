using Demo.DbRefreshManager.Application.Repositories.Base;
using Demo.DbRefreshManager.Domain.Entities.Users;

namespace Demo.DbRefreshManager.Application.Repositories;

/// <summary>
/// Репозиторий пользователей.
/// </summary>
public interface IUsersRepository : IRepository<User>
{
    /// <summary>
    /// Мердж пользователя из LDAP.
    /// </summary>
    /// <param name="ldapUser">Данные пользователя ldap.</param>
    /// <returns>Пользователь из БД.</returns>
    Task<User> MergeLdapUser(User ldapUser);
}

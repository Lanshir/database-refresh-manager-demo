using Demo.DbRefreshManager.Dal.Entities.Users;
using Demo.DbRefreshManager.Dal.Repositories.Abstract.Base;

namespace Demo.DbRefreshManager.Dal.Repositories.Abstract;

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

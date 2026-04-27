using Demo.DbRefreshManager.Domain.Models.ActiveDirectory;
using Demo.DbRefreshManager.Domain.Models.Users;

namespace Demo.DbRefreshManager.Domain.Mappings;

public static class LdapUserMappings
{
    extension(LdapUser src)
    {
        /// <summary>
        /// Конвертация модели пользователя из LDAP в доменную.
        /// </summary>
        public User ToDomainUser() => new()
        {
            FirstName = src.FirstName,
            LastName = src.LastName,
            Patronymic = src.Patronymic,
            Email = src.Email,
            LdapDn = src.Dn,
            LdapLogin = src.Login,
            LdapChangeDate = src.WhenChanged,
            Roles = [.. src.Groups.Select(g => new UserRole { LdapGroup = g })]
        };
    }
}

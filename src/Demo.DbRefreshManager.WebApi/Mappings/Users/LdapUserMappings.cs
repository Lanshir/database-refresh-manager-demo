using Demo.DbRefreshManager.Dal.Entities.Users;
using Demo.DbRefreshManager.Services.Models.ActiveDirectory;

namespace Demo.DbRefreshManager.WebApi.Mappings.Users;

public static class LdapUserMappings
{
    extension(LdapUser src)
    {
        /// <summary>
        /// Конвертация модели пользователя из LDAP в доменную.
        /// </summary>
        public User ToDto() => new()
        {
            LdapDn = src.Dn,
            LdapLogin = src.Login,
            LdapChangeDate = src.WhenChanged,
            Roles = [.. src.Groups.Select(g => new UserRole { LdapGroup = g })]
        };
    }
}

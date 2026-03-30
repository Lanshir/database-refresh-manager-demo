using Demo.DbRefreshManager.Dal.Entities.Users;

namespace Demo.DbRefreshManager.Dal.Mappings;

public static class UserMappings
{
    extension(User src)
    {
        /// <summary>
        /// Перенести данные пользователя на другого пользователя.
        /// </summary>
        public User MergeTo(User target)
        {
            target.FirstName = src.FirstName;
            target.Patronymic = src.Patronymic;
            target.LastName = src.LastName;
            target.Email = src.Email;
            target.LdapLogin = src.LdapLogin;
            target.LdapDn = src.LdapDn;
            target.LdapChangeDate = src.LdapChangeDate;
            target.CreationDate = src.CreationDate;
            target.ModifyDate = src.ModifyDate;

            return target;
        }
    }
}

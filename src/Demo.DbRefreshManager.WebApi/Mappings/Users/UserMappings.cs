using Demo.DbRefreshManager.Domain.Entities.Users;
using Demo.DbRefreshManager.WebApi.Models.Auth;

namespace Demo.DbRefreshManager.WebApi.Mappings.Users;

public static class UserMappings
{
    extension(User src)
    {
        /// <summary>
        /// Конвертация модели пользователя в dto результата авторизации.
        /// </summary>
        public LoginResultDto ToLoginResultDto() => new(
            Login: src.LdapLogin,
            FullName: string.Join(" ", [src.LastName, src.FirstName, src.Patronymic]).Trim(),
            Roles: [.. src.Roles.Select(r => r.Name)]
            );
    }
}

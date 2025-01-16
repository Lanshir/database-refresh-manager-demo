using AutoMapper;
using Demo.DbRefreshManager.Dal.Entities.Users;
using Demo.DbRefreshManager.WebApi.Models.Authorization;

namespace Demo.DbRefreshManager.WebApi.Mappings.Users;

/// <summary>
/// Профиль конвертации модели пользователя в dto результата авторизации.
/// </summary>
public class UserToLoginResultDtoMapping : Profile
{
    /// <inheritdoc cref="UserToLoginResultDtoMapping" />
    public UserToLoginResultDtoMapping()
    {
        CreateMap<User, LoginResultDto>()
            .ForMember(dto => dto.Login,
                act => act.MapFrom(u => u.LdapLogin))
            .ForMember(dto => dto.Roles,
                act => act.MapFrom(u => u.Roles.Select(r => r.Name)))
            .ForMember(dto => dto.FullName,
                    act => act.MapFrom(u =>
                        string.Join(" ", new string?[] { u.LastName, u.FirstName, u.Patronymic })));
    }
}

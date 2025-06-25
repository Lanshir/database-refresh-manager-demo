using AutoMapper;
using Demo.DbRefreshManager.Dal.Entities.Users;

namespace Demo.DbRefreshManager.WebApi.Mappings.Users;

/// <summary>
/// Профиль копирования свойств модели пользователя.
/// </summary>
public class UserMapping : Profile
{
    /// <inheritdoc cref="UserMapping" />
    public UserMapping()
    {
        CreateMap<User, User>()
            .ForMember(u => u.Id, act => act.Ignore())
            .ForMember(u => u.Roles, act => act.Ignore());
    }
}

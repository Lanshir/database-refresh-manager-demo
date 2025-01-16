using AutoMapper;
using Demo.DbRefreshManager.Dal.Entities.Users;
using Demo.DbRefreshManager.Services.Models.ActiveDirectory;

namespace Demo.DbRefreshManager.WebApi.Mappings.Users;
/// <summary>
/// Профиль конвертации модели пользователя ldap в доменную.
/// </summary>
public class LdapUserToUserMapping : Profile
{
    /// <inheritdoc cref="LdapUserToUserMapping" />
    public LdapUserToUserMapping()
    {
        CreateMap<LdapUser, User>()
            .ForMember(u => u.LdapDn,
                act => act.MapFrom(ldap => ldap.Dn))
            .ForMember(u => u.LdapLogin,
                act => act.MapFrom(ldap => ldap.Login))
            .ForMember(u => u.LdapChangeDate,
                act => act.MapFrom(ldap => ldap.WhenChanged))
            .ForMember(u => u.Roles,
                act => act.MapFrom(ldap => ldap.Groups
                    .Select(g => new UserRole { LdapGroup = g })));
    }
}

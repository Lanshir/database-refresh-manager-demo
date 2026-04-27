using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Domain.Models.ActiveDirectory;
using Demo.DbRefreshManager.Domain.Models.Users;

namespace Demo.DbRefreshManager.Application.Features.Users;

/// <summary>
/// Вставка/обновление пользователя LDAP в БД.
/// </summary>
public interface IMergeLdapUserToDbCommandHandler : IAsyncHandler<User, LdapUser>;
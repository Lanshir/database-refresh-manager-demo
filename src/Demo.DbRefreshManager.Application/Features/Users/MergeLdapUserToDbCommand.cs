using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Domain.Entities.ActiveDirectory;
using Demo.DbRefreshManager.Domain.Entities.Users;

namespace Demo.DbRefreshManager.Application.Features.Users;

/// <summary>
/// Команда вставки/обновления пользователя LDAP в БД.
/// </summary>
public interface IMergeLdapUserToDbCommandHandler : IAsyncHandler<User, LdapUser>;
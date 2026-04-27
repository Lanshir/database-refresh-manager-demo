using Demo.DbRefreshManager.Application.Models.Options;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;
using Demo.DbRefreshManager.Domain.Errors;
using Demo.DbRefreshManager.Domain.Models.ActiveDirectory;
using Microsoft.Extensions.Options;

namespace Demo.DbRefreshManager.Application.Features.Auth;

/// <summary>
/// Вход пользователя в домен.
/// </summary>
public interface ILoginToDomainCommandHandler
    : IHandler<Result<LdapUser>, LoginToDomain.Command>;

public static class LoginToDomain
{
    public record struct Command(string Login, string Password);

    internal class CommandHandler(
        IDomainControllerService domainController,
        IOptions<LdapOptions> ldapOptions
        ) : ILoginToDomainCommandHandler
    {
        public Result<LdapUser> Handle(Command cmd)
        {
            domainController.Connect(ldapOptions.Value.Host, 5, 2000);
            domainController.Authenticate(cmd.Login, cmd.Password);

            if (!domainController.IsAuthenticated)
                return AuthErrors.BadCredentials;

            var ldapUser = domainController.GetUserData(
                cmd.Login, ldapOptions.Value.UserSearchDnBases);

            if (ldapUser == null)
                return AuthErrors.LdapUserNotFound;

            return ldapUser;
        }
    }
}

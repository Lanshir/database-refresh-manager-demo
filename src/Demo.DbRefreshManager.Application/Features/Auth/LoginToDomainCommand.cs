using Demo.DbRefreshManager.Application.Models.Options;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;
using Demo.DbRefreshManager.Domain.Entities.ActiveDirectory;
using Demo.DbRefreshManager.Domain.Errors;
using Microsoft.Extensions.Options;

namespace Demo.DbRefreshManager.Application.Features.Auth;

/// <summary>
/// Команда входа пользователя в домен.
/// </summary>
public interface ILoginToDomainCommandHandler
    : IHandler<Result<LdapUser>, LoginToDomainCommand.Dto>;

public static class LoginToDomainCommand
{
    public record struct Dto(string Login, string Password);

    public class Handler(
        IDomainControllerService domainController,
        IOptions<LdapOptions> ldapOptions
        ) : ILoginToDomainCommandHandler
    {
        public Result<LdapUser> Handle(Dto cmd)
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

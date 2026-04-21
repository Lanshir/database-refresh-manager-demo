using Demo.DbRefreshManager.Application.Models.Options;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;
using Demo.DbRefreshManager.Domain.Entities.ActiveDirectory;
using Demo.DbRefreshManager.Domain.Errors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Demo.DbRefreshManager.Application.Features.Auth;

/// <summary>
/// Команда входа пользователя в домен.
/// </summary>
public interface ILoginToDomainCommandHandler
    : IHandler<Result<LdapUser>, LoginToDomainCommand.InputDto>;

public static class LoginToDomainCommand
{
    public record InputDto(string Login, string Password);

    public class Handler(
        IHostEnvironment environment,
        IDomainControllerService domainController,
        IOptions<LdapOptions> ldapOptions
        ) : ILoginToDomainCommandHandler
    {
        public Result<LdapUser> Handle(InputDto input)
        {
            try
            {
                domainController.Connect(ldapOptions.Value.Host, 5, 2000);
                domainController.Authenticate(input.Login, input.Password);

                if (!domainController.IsAuthenticated)
                    return AuthErrors.BadCredentials;

                var ldapUser = domainController.GetUserData(
                    input.Login, ldapOptions.Value.UserSearchDnBases);

                if (ldapUser == null)
                    return AuthErrors.LdapUserNotFound;

                return Result.Success(ldapUser);
            }
            catch (Exception ex)
            {
                if (environment.IsDevelopment())
                {
                    Debug.WriteLine(ex);
                }

                return AuthErrors.Unexpected;
            }
        }
    }
}

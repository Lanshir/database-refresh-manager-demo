using Demo.DbRefreshManager.Application.Features.Users;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Domain.Errors;
using Demo.DbRefreshManager.WebApi.Endpoints.Abstract;
using Demo.DbRefreshManager.WebApi.Infrastructure.HttpResults;
using Demo.DbRefreshManager.WebApi.Infrastructure.Options;
using Demo.DbRefreshManager.WebApi.Mappings.Users;
using Demo.DbRefreshManager.WebApi.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;

namespace Demo.DbRefreshManager.WebApi.Endpoints;

public class LoginEndpointsV1 : IEndpointsMapper
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder)
    {
        var grp = builder.MapGroup("auth")
            .WithTags("Login")
            .MapToApiVersion(1);

        grp.MapGet("login", CheckAuth)
            .RequireAuthorization()
            .WithSummary("Проверка авторизации.");

        grp.MapPost("login", Login)
            .WithSummary("Авторизация");

        grp.MapDelete("login", (Delegate)Logout)
            .WithSummary("Деавторизация.");

        return grp;
    }

    private static async Task<Ok<LoginResultDto>> CheckAuth(
        IUserIdentityProvider userIdentity)
        => TypedResults.Ok(
            new LoginResultDto(
                Login: userIdentity.GetUserLogin(),
                FullName: userIdentity.GetUserFullName(),
                Roles: userIdentity.GetRoles()));

    private static async Task<Ok> Logout(HttpContext ctx)
    {
        await ctx.SignOutAsync();

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<LoginResultDto>, ProblemHttpResult>> Login(
        HttpContext httpContext,
        IWebHostEnvironment environment,
        IUserIdentityProvider userIdentity,
        IDomainControllerService domainController,
        IMergeLdapUserToDbCommandHandler mergeLdapUserToDbCmd,
        IOptions<LdapOptions> ldapOptions,
        LoginInputDto input,
        CancellationToken ct)
    {
        try
        {
            if (userIdentity.IsAuthenticated())
            {
                await httpContext.SignOutAsync();
            }

            // Доменная авторизация, получение данных пользователя.
            domainController.Connect(ldapOptions.Value.Host, 5, 2000);
            domainController.Authenticate(input.Login, input.Password);

            if (!domainController.IsAuthenticated)
            {
                return TypedResults.Problem(new ExtendedProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = AuthErrors.Title,
                    ErrorCode = AuthErrors.BadCredentials.Code,
                    Detail = AuthErrors.BadCredentials.Message
                });
            }

            var ldapUser = domainController.GetUserData
                (input.Login, ldapOptions.Value.UserSearchDnBases);

            if (ldapUser == null)
            {
                return TypedResults.Problem(
                    new ExtendedProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = AuthErrors.Title,
                        ErrorCode = AuthErrors.LdapUserNotFound.Code,
                        Detail = AuthErrors.LdapUserNotFound.Message
                    });
            }

            var dbUser = await mergeLdapUserToDbCmd.HandleAsync(ldapUser, ct);
            var dto = dbUser.ToLoginResultDto();

            var claims = userIdentity.CreateClaimsList(
                userId: dbUser.Id,
                login: dto.Login,
                fullName: dto.FullName,
                roles: [.. dto.Roles]);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Аутентификация.
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = input.RememberMe
                });

            return TypedResults.Ok(dto);
        }
        catch (Exception ex)
        {
            if (environment.IsDevelopment())
            {
                Debug.WriteLine(ex);
            }

            return TypedResults.Problem(new ExtendedProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = AuthErrors.Title,
                ErrorCode = AuthErrors.Unexpected.Code,
                Detail = AuthErrors.Unexpected.Message
            });
        }
    }
}

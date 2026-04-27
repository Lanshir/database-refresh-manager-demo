using Demo.DbRefreshManager.Application.Features.Auth;
using Demo.DbRefreshManager.Application.Features.Users;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.WebApi.Endpoints.Abstract;
using Demo.DbRefreshManager.WebApi.Mappings.Results;
using Demo.DbRefreshManager.WebApi.Mappings.Users;
using Demo.DbRefreshManager.WebApi.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
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
        ILoginToDomainCommandHandler loginToDomain,
        IMergeLdapUserToDbCommandHandler mergeLdapUserToDb,
        LoginInputDto input,
        CancellationToken ct)
    {
        if (userIdentity.IsAuthenticated())
        {
            await httpContext.SignOutAsync();
        }

        var ldapLoginResult = loginToDomain.Handle(new(input.Login, input.Password));

        if (ldapLoginResult.IsFailure)
        {
            return TypedResults.Problem(
                ldapLoginResult.ToProblemDetails(
                    "Ошибка аутентификации",
                    StatusCodes.Status401Unauthorized));
        }

        var ldapUser = ldapLoginResult.Value!;
        var dbUser = await mergeLdapUserToDb.HandleAsync(ldapUser, ct);
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
}

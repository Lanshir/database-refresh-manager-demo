using Demo.DbRefreshManager.Application.Features.Users;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Domain.Entities.ActiveDirectory;
using Demo.DbRefreshManager.WebApi.Endpoints.Abstract;
using Demo.DbRefreshManager.WebApi.Mappings.Users;
using Demo.DbRefreshManager.WebApi.Models.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
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
        IMergeLdapUserToDbCommandHandler mergeLdapUserToDbCmd,
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
            /*
            domainController.Connect(appConfig.Ldap.Host, 5, 2000);
            domainController.Authorize(input.Login, input.Password);

            if (!domainController.IsAuthorized)
            {
                return Unauthorized(ApiResponse.Error(
                    "Ошибка авторизации, проверьте логин/пароль."));
            }

            var ldapUser = domainController.GetUserData
                (input.Login, appConfig.Ldap.UserSearchDnBases);

            if (ldapUser == null)
            {
                return Unauthorized(ApiResponse.Error(
                    "Не удалось получить данные пользователя."));
            }
            */

            // DEMO AUTH.
            var ldapUser = _demoUsers.FirstOrDefault(u => u.Login == input.Login);

            if (ldapUser == null || input.Password != "pwd")
            {
                return TypedResults.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Ошибка аутентификации",
                    detail: "Ошибка входа, проверьте логин/пароль");
            }

            var dbUser = await mergeLdapUserToDbCmd.HandleAsync(ldapUser, ct);
            var dto = dbUser.ToLoginResultDto();

            var claims = userIdentity.CreateClaimsList(
                dbUser.Id,
                dto.Login,
                dto.FullName,
                [.. dto.Roles]);

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

            return TypedResults.Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Ошибка аутентификации",
                detail: "При попытке входа произошла неожиданная ошибка, попробуйте позже");
        }
    }

    private static readonly List<LdapUser> _demoUsers = new()
    {
        {
            new LdapUser
            {
                Dn = "demoMaster",
                Login = "demoMaster",
                Company = "Рога и копыта",
                Department = "Отдел прокрастинации",
                Email = "demo@demo.ru",
                FirstName = "Иван",
                Patronymic = "Иванович",
                LastName = "Иванов",
                FullName = "Иванов Иван Иванович",
                Position = "Мастер",
                WhenChanged = DateTime.UtcNow,
                Groups = ["RefreshManagerMaster"]
            }
        },
        {
            new LdapUser
            {
                Dn = "demoAnalyst",
                Login = "demoAnalyst",
                Company = "Рога и копыта",
                Department = "Отдел прокрастинации",
                Email = "demo@demo.ru",
                FirstName = "Иван",
                Patronymic = "Иванович",
                LastName = "Иванов",
                FullName = "Иванов Иван Иванович",
                Position = "Аналитик",
                WhenChanged = DateTime.UtcNow,
                Groups = ["RefreshManagerAnalyst"]
            }
        },
        {
            new LdapUser
            {
                Dn = "demoSupport",
                Login = "demoSupport",
                Company = "Рога и копыта",
                Department = "Отдел прокрастинации",
                Email = "demo@demo.ru",
                FirstName = "Иван",
                Patronymic = "Иванович",
                LastName = "Иванов",
                FullName = "Иванов Иван Иванович",
                Position = "Тех. поддержка",
                WhenChanged = DateTime.UtcNow,
                Groups = ["RefreshManagerSupport"]
            }
        }
    };
}

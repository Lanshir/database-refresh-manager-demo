using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Demo.DbRefreshManager.Services.Models.ActiveDirectory;
using Demo.DbRefreshManager.WebApi.Endpoints.Abstract;
using Demo.DbRefreshManager.WebApi.Infrastructure.Helpers.Abstract;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;
using Demo.DbRefreshManager.WebApi.Mappings.Users;
using Demo.DbRefreshManager.WebApi.Models.Api;
using Demo.DbRefreshManager.WebApi.Models.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using System.Security.Claims;

namespace Demo.DbRefreshManager.WebApi.Endpoints;

public class LoginEndpointsV1 : IEndpointGroupSetup
{
    public RouteGroupBuilder AddEndpointGroupSetup(RouteGroupBuilder builder)
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

    private static async Task<Ok<ApiResponseDto<LoginResultDto>>> CheckAuth(
        IUserIdentityHelper userIdentity)
        => TypedResults.Ok(
            ApiResponse.Success(
                new LoginResultDto(
                    Login: userIdentity.GetUserLogin(),
                    FullName: userIdentity.GetUserFullName(),
                    Roles: userIdentity.GetRoles())));

    private static async Task<Ok<ApiResponseDto<object>>> Logout(HttpContext ctx)
    {
        await ctx.SignOutAsync();

        return TypedResults.Ok(ApiResponse.Success());
    }

    private static async Task<Results<
        Ok<ApiResponseDto<LoginResultDto>>,
        JsonHttpResult<ApiResponseDto<object>>
    >> Login(
        HttpContext httpContext,
        IWebHostEnvironment environment,
        IUserIdentityHelper userIdentity,
        IUsersRepository usersRepository,
        LoginInputDto input)
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
            var ldapUser = _demoUsers.FirstOrDefault(u => u.Key.Equals(input.Login)).Value;

            if (ldapUser == null || input.Password != "pwd")
            {
                return TypedResults.Json(ApiResponse.Error(
                    "Ошибка авторизации, проверьте логин/пароль."),
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var dbUser = await usersRepository.MergeLdapUser(ldapUser.ToDomainUser());
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

            return TypedResults.Ok(ApiResponse.Success(dto));
        }
        catch (Exception ex)
        {
            if (environment.IsDevelopment())
            {
                Debug.WriteLine(ex);
            }

            return TypedResults.Json(
                ApiResponse.Error("Ошибка авторизации."),
                statusCode: StatusCodes.Status401Unauthorized);
        }
    }

    private static readonly Dictionary<string, LdapUser> _demoUsers = new()
    {
        {
            "demoMaster",
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
            "demoAnalyst",
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
            "demoSupport",
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

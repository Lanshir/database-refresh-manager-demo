using Asp.Versioning;
using Demo.DbRefreshManager.Common.Config.Abstract;
using Demo.DbRefreshManager.Common.Converters.Abstract;
using Demo.DbRefreshManager.Dal.Entities.Users;
using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Demo.DbRefreshManager.Services.Abstract;
using Demo.DbRefreshManager.Services.Models.ActiveDirectory;
using Demo.DbRefreshManager.WebApi.Infrastructure.Helpers.Abstract;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;
using Demo.DbRefreshManager.WebApi.Models.Api;
using Demo.DbRefreshManager.WebApi.Models.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Demo.DbRefreshManager.WebApi.Controllers.Authorization;

/// <summary>
/// Контроллер авторизации.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthorizationController(
    IAppConfig appConfig,
    ITypeMapper mapper,
    IUserIdentityHelper identityService,
    IDomainControllerService domainController,
    IWebHostEnvironment environment,
    IUsersRepository usersRepository
    ) : ControllerBase
{

    /// <summary>
    /// Проверка авторизации.
    /// </summary>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<LoginResultDto>>> Get()
    {
        return ApiResponse.Success(new LoginResultDto
        {
            Login = identityService.GetUserLogin(),
            FullName = identityService.GetUserFullName(),
            Roles = identityService.GetRoles()
        });
    }

    /// <summary>
    /// Авторизация.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponseDto<LoginResultDto>>> Post(LoginInputDto input)
    {
        try
        {
            if (identityService.IsAuthenticated())
            {
                await HttpContext.SignOutAsync();
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
                return Unauthorized(ApiResponse.Error(
                    "Ошибка авторизации, проверьте логин/пароль."));
            }

            var dbUser = mapper.Map<User>(ldapUser);

            dbUser = await usersRepository.MergeLdapUser(dbUser);

            var dto = mapper.Map<LoginResultDto>(dbUser);

            var claims = identityService.CreateClaimsList(
                dbUser.Id,
                dto.Login,
                dto.FullName,
                dto.Roles.ToList());

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Аутентификация.
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = input.RememberMe
                });

            return ApiResponse.Success(dto);
        }
        catch (Exception ex)
        {
            if (environment.IsDevelopment())
            {
                Debug.WriteLine(ex);
            }

            return Unauthorized(ApiResponse.Error("Ошибка авторизации."));
        }
    }

    /// <summary>
    /// Деавторизация.
    /// </summary>
    [HttpDelete]
    public async Task<ActionResult<ApiResponseDto<object>>> Delete()
    {
        await HttpContext.SignOutAsync();

        return ApiResponse.Success();
    }

    private readonly Dictionary<string, LdapUser> _demoUsers = new()
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

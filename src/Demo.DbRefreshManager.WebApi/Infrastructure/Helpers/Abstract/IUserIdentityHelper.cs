using System.Security.Claims;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Helpers.Abstract;

/// <summary>
/// Абстракция для работы с Identity пользователя.
/// </summary>
public interface IUserIdentityHelper
{
    /// <summary>
    /// Создание списка Claim для авторизации.
    /// </summary>
    /// <param name="userId">Id пользователя.</param>
    /// <param name="login">Логин.</param>
    /// <param name="fullName">Полное имя.</param>
    /// <param name="roles">Список ролей.</param>
    List<Claim> CreateClaimsList(
        int userId,
        string login,
        string fullName,
        List<string> roles);

    /// <summary>
    /// Получить id пользователя.
    /// </summary>
    int GetUserId();

    /// <summary>
    /// Получить полное имя пользователя.
    /// </summary>
    string GetUserFullName();

    /// <summary>
    /// Получить логин пользователя.
    /// </summary>
    string GetUserLogin();

    /// <summary>
    /// Получить роли пользователя.
    /// </summary>
    List<string> GetRoles();

    /// <summary>
    /// Пользователь авторизован.
    /// </summary>
    bool IsAuthenticated();
}

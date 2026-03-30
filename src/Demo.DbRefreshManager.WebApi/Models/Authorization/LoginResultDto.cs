namespace Demo.DbRefreshManager.WebApi.Models.Authorization;

/// <summary>
/// Модель dto данных результата авторизации.
/// </summary>
/// <param name="Login">Логин пользователя.</param>
/// <param name="FullName">ФИО пользователя.</param>
/// <param name="Roles">Роли пользователя.</param>
public record LoginResultDto(
    string Login,
    string FullName,
    List<string> Roles
    )
{ }

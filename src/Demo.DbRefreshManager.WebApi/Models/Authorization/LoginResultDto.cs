namespace Demo.DbRefreshManager.WebApi.Models.Authorization;

/// <summary>
/// Модель dto данных результата авторизации.
/// </summary>
public class LoginResultDto
{
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// ФИО пользователя.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Роли пользователя.
    /// </summary>
    public IEnumerable<string> Roles { get; set; } = new string[0];
}

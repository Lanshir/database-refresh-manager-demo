namespace Demo.DbRefreshManager.WebApi.Models.Authorization;

/// <summary>
/// Модель dto данных ввода авторизации.
/// </summary>
public class LoginInputDto
{
    /// <summary>
    /// Логин.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Запомнить авторизацию.
    /// </summary>
    public bool RememberMe { get; set; }
}

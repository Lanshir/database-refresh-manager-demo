namespace Demo.DbRefreshManager.WebApi.Models.Authorization;

/// <summary>
/// Модель dto данных ввода авторизации.
/// </summary>
/// <param name="Login">Логин.</param>
/// <param name="Password">Пароль.</param>
/// <param name="RememberMe">Запомнить авторизацию.</param>
public record LoginInputDto(string Login, string Password, bool RememberMe)
{
    public LoginInputDto() : this(string.Empty, string.Empty, false) { }
};

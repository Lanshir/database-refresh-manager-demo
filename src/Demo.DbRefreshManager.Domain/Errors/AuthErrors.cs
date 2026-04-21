using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Domain.Errors;

/// <summary>
/// Ошибки аутентификации.
/// </summary>
public static class AuthErrors
{
    private const string _prefix = "Auth";

    public const string Title = "Ошибка аутентификации";

    public static readonly Error BadCredentials = new(
        Code: $"{_prefix}.BadCredentials",
        Message: "Ошибка входа, проверьте логин/пароль");

    public static readonly Error LdapUserNotFound = new(
        Code: $"{_prefix}.LdapUserNotFound",
        Message: "Не удалось получить данные пользователя в домене");

    public static readonly Error Unexpected = new(
        Code: $"{_prefix}.Unexpected",
        Message: "При попытке входа произошла неожиданная ошибка, попробуйте позже");
}

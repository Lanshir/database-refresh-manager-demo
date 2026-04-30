using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Domain.Errors;

/// <summary>
/// Ошибки задач на перезаливку.
/// </summary>
public static class DbRefreshJobErrors
{
    private const string _prefix = "DbRefreshJob";

    public static readonly Error Forbidden = new(
        Code: $"{_prefix}.Forbidden",
        Message: "У пользователя не прав для изменения задачи");

    public static readonly Error NotFound = new(
        Code: $"{_prefix}.NotFound",
        Message: "Задача на перезаливку не найдена");
}

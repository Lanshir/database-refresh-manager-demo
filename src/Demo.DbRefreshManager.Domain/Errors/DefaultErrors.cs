using Demo.DbRefreshManager.Core.Results;

namespace Demo.DbRefreshManager.Domain.Errors;

/// <summary>
/// Стандартные ошибки приложения.
/// </summary>
public static class DefaultErrors
{
    private const string _prefix = "Default";

    public static readonly Error Unexpected = new(
        Code: $"{_prefix}.Unexpected",
        Message: "Произошла непредвиденная ошибка приложения");
}

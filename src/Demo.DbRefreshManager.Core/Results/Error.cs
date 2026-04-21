namespace Demo.DbRefreshManager.Core.Results;

/// <summary>
/// Ошибка приложения.
/// </summary>
public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
};

/// <summary>
/// Ошибка валидации.
/// </summary>
public record ValidationError(string Code, string Message, Dictionary<string, string> Details)
    : Error(Code, Message);

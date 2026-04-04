namespace Demo.DbRefreshManager.WebApi.Infrastructure.Options;

/// <summary>
/// Конфигурация Cookie авторизации.
/// </summary>
public record AuthCookieOptions
{
    /// <summary>
    /// Время жизни Cookie в минутах.
    /// </summary>
    public int LifetimeMinutes { get; init; }
}

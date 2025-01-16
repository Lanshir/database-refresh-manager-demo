using Demo.DbRefreshManager.Common.Config.Concrete;

namespace Demo.DbRefreshManager.Common.Config.Abstract;

/// <summary>
/// Конфигурация приложения из внешних источников.
/// </summary>
public interface IAppConfig
{
    /// <summary>
    /// Время жизни авторизационной куки в минутах.
    /// </summary>
    int AuthCookieLifetimeMinutes { get; }

    /// <summary>
    /// Строка подключения к БД.
    /// </summary>
    string DbConnectionString { get; }

    /// <inheritdoc cref="Concrete.FrontendConfig" />
    FrontendConfig FrontendConfig { get; }
}

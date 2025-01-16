using Demo.DbRefreshManager.Common.Config.Abstract;
using Demo.DbRefreshManager.Common.Config.Concrete;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Config;

/// <inheritdoc cref="IAppConfig" />
public class AppConfig : IAppConfig
{
    public int AuthCookieLifetimeMinutes { get; init; }

    public string DbConnectionString { get; init; } = string.Empty;

    public FrontendConfig FrontendConfig { get; init; } = new();

    /// <inheritdoc cref="AppConfig" />
    public AppConfig(IConfiguration configuration)
        => configuration.Bind(this, options => options.BindNonPublicProperties = true);
}

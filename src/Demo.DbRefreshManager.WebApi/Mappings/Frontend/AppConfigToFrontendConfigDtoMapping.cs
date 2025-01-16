using AutoMapper;
using Demo.DbRefreshManager.Common.Config.Abstract;
using Demo.DbRefreshManager.WebApi.Models.Frontend;

namespace Demo.DbRefreshManager.WebApi.Mappings.Frontend;

/// <summary>
/// Профиль конвертации конфигурации приложения в dto конфигурации frontend.
/// </summary>
public class AppConfigToFrontendConfigDtoMapping : Profile
{
    /// <inheritdoc cref="AppConfigToFrontendConfigDtoMapping" />
    public AppConfigToFrontendConfigDtoMapping()
    {
        CreateMap<IAppConfig, FrontendConfigDto>()
            .IncludeMembers(cfg => cfg.FrontendConfig);
    }
}

using AutoMapper;
using Demo.DbRefreshManager.Common.Config.Concrete;
using Demo.DbRefreshManager.WebApi.Models.Frontend;

namespace Demo.DbRefreshManager.WebApi.Mappings.Frontend;

/// <summary>
/// Профиль конвертации доменной модели конфигурации frontend в dto.
/// </summary>
public class FrontendConfigToDtoMapping : Profile
{
    /// <inheritdoc cref="FrontendConfigToDtoMapping" />
    public FrontendConfigToDtoMapping()
    {
        CreateMap<FrontendConfig, FrontendConfigDto>();
    }
}

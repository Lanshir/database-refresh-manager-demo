using Demo.DbRefreshManager.Common.Config.Abstract;
using Demo.DbRefreshManager.WebApi.GraphQL.Queries.Base;
using Demo.DbRefreshManager.WebApi.Mappings.Frontend;
using Demo.DbRefreshManager.WebApi.Models.Frontend;

namespace Demo.DbRefreshManager.WebApi.GraphQL.Queries.V1;

[ExtendObjectType<QueryV1>]
public class FrontendConfigsQueriesV1
{
    /// <summary>
    /// Получить конфигурацию frontend.
    /// </summary>
    public async Task<FrontendConfigDto> GetFrontendConfigs(IAppConfig appConfig)
        => appConfig.FrontendConfig.ToDto();
}

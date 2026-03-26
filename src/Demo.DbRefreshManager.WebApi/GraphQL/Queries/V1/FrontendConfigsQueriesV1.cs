using Demo.DbRefreshManager.Common.Config.Abstract;
using Demo.DbRefreshManager.Common.Converters.Abstract;
using Demo.DbRefreshManager.WebApi.GraphQL.Queries.Base;
using Demo.DbRefreshManager.WebApi.Models.Frontend;

namespace Demo.DbRefreshManager.WebApi.GraphQL.Queries.V1;

[ExtendObjectType<QueryV1>]
public class FrontendConfigsQueriesV1
{
    /// <summary>
    /// Получить конфигурацию frontend.
    /// </summary>
    public async Task<FrontendConfigDto> GetFrontendConfigs(
        IAppConfig appConfig,
        ITypeMapper mapper)
        => mapper.Map<FrontendConfigDto>(appConfig);
}

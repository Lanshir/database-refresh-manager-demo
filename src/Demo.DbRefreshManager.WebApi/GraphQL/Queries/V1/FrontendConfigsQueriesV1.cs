using Demo.DbRefreshManager.WebApi.GraphQL.Queries.Base;
using Demo.DbRefreshManager.WebApi.Mappings.Frontend;
using Demo.DbRefreshManager.WebApi.Models.Frontend;
using Demo.DbRefreshManager.WebApi.Models.Options;
using Microsoft.Extensions.Options;

namespace Demo.DbRefreshManager.WebApi.GraphQL.Queries.V1;

[ExtendObjectType<QueryV1>]
public class FrontendConfigsQueriesV1
{
    /// <summary>
    /// Получить конфигурацию frontend.
    /// </summary>
    public async Task<FrontendConfigDto> GetFrontendConfigs(IOptions<FrontendOptions> options)
        => options.Value.ToDto();
}

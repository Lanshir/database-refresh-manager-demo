namespace Demo.DbRefreshManager.WebApi.Endpoints.Abstract;

/// <summary>
/// Конфигуратор группы эндпоинтов Minimal Api.
/// </summary>
public interface IEndpointGroupSetup
{
    RouteGroupBuilder AddEndpointGroupSetup(RouteGroupBuilder builder);
}

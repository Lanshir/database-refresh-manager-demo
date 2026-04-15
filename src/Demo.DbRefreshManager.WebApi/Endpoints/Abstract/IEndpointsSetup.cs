namespace Demo.DbRefreshManager.WebApi.Endpoints.Abstract;

/// <summary>
/// Конфигуратор эндпоинтов Minimal Api.
/// </summary>
public interface IEndpointsSetup
{
    IEndpointRouteBuilder SetupEndpoints(IEndpointRouteBuilder builder);
}

namespace Demo.DbRefreshManager.WebApi.Endpoints.Abstract;

/// <summary>
/// Конфигуратор эндпоинтов Minimal Api.
/// </summary>
public interface IEndpointsMapper
{
    IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder);
}

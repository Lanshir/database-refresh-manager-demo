using Demo.DbRefreshManager.Common.Exceptions;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.MinimalApi;

/// <summary>
/// Обработка ошибок IEndpoint.
/// </summary>
public class EndpointExceptionsFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
    {
        var logger = ctx.HttpContext.RequestServices
            .GetRequiredService<ILogger<EndpointExceptionsFilter>>();

        try
        {
            return await next(ctx);
        }
        // Ошибка бизнес логики возвращает ApiResponseDto со статусом 400 и данными ошибки.
        catch (BusinessLogicException exc)
        {
            var resultDto = ApiResponse.Default(exc.Code, exc.ErrorData, exc.Message);

            return Results.Json(resultDto, statusCode: StatusCodes.Status400BadRequest);
        }
        // Другие ошибки возвращают статус 500 и dto ошибки.
        catch (Exception exc)
        {
            var resultDto = ApiResponse.Error();

            logger.LogError(exc, "Unhandled endpoint exception at {path}", ctx.HttpContext.Request.Path);

            return Results.Json(resultDto, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}

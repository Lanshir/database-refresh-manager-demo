using Demo.DbRefreshManager.Common.Exceptions;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;
using Microsoft.AspNetCore.Mvc;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Endpoints;

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

        var instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";

        try
        {
            return await next(ctx);
        }
        // Ошибка бизнес логики возвращает статус 400, без логирования.
        catch (BusinessLogicException exc)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "BusinessLogic.Error",
                detail: exc.Message,
                instance: instance);
        }
        // Другие ошибки возвращают статус 500 и логирует ошибку.
        catch (Exception exc)
        {
            logger.LogError(exc, "Unhandled endpoint exception at {path}", ctx.HttpContext.Request.Path);

            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Default.UnexpectedError",
                detail: "Unexpected error occured, view logs for details",
                instance: instance);
        }
    }
}

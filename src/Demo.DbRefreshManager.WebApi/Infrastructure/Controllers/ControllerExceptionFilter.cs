using Demo.DbRefreshManager.Common.Exceptions;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Controllers;

/// <summary>
/// Глобальная обработка Excepton контроллеров.
/// </summary>
public class ControllerExceptionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext ctx) { }

    public void OnActionExecuted(ActionExecutedContext ctx)
    {
        if (ctx.Exception == null) return;

        // Ошибка бизнес логики возвращает ApiesponseDto со статусом 400 и данными ошибки.
        if (ctx.Exception is BusinessLogicException exc)
        {
            ctx.Result = new BadRequestObjectResult(
                ApiResponse.Default(exc.Code, exc.Data, exc.Message));
        }
        // Другие ошибки возвращают статус 500 и dto ошибки.
        else
        {
            ctx.Result = new ObjectResult(ApiResponse.Error())
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        ctx.ExceptionHandled = true;
    }
}

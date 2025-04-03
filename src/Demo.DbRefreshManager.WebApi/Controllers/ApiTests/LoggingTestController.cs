using Asp.Versioning;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;
using Demo.DbRefreshManager.WebApi.Models.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.DbRefreshManager.WebApi.Controllers.ApiTests;

/// <summary>
/// Контроллер проверки логирования.
/// </summary>
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class LoggingTestController(ILogger<LoggingTestController> logger) : ControllerBase
{
    /// <summary>
    /// Вызов тестовых сообщений логера.
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponseDto<object>>> Post()
    {
        logger.LogDebug("debug logging test");
        logger.LogInformation("info logging test");
        logger.LogWarning("warning logging test");

        var testObj = new { a = 1, b = "test string", c = new int[2] { 1, 2 } };

        logger.LogInformation("serilog object logging test {@obj}", testObj);

        try
        {
            throw new Exception("Exception throw test");
        }
        catch (Exception exc)
        {
            logger.LogError(exc, "Exception logging test");
        }

        return ApiResponse.Success();
    }
}

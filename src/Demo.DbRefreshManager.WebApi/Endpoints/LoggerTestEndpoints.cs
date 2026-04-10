using Demo.DbRefreshManager.WebApi.Endpoints.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Demo.DbRefreshManager.WebApi.Endpoints;

public class LoggerTestEndpoints : IEndpointGroupSetup
{
    public RouteGroupBuilder AddEndpointGroupSetup(RouteGroupBuilder builder)
    {
        var grp = builder.MapGroup("logging")
            .WithTags("Logging")
            .WithSummary("Логирование api.");

        grp.MapPost("test", RunLoggerTest)
            .WithSummary("Вызов тестовых сообщений логгера.");

        return grp;
    }

    private static async Task<Ok> RunLoggerTest(
        ILogger<LoggerTestEndpoints> logger)
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

        return TypedResults.Ok();
    }
}

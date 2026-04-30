using Demo.DbRefreshManager.Application;
using Demo.DbRefreshManager.Infrastructure;
using Demo.DbRefreshManager.Infrastructure.Db;
using Demo.DbRefreshManager.WebApi.Endpoints.Extensions;
using Demo.DbRefreshManager.WebApi.Infrastructure.Constants;
using Demo.DbRefreshManager.WebApi.Infrastructure.Extensions;
using Demo.DbRefreshManager.WebApi.Infrastructure.Healthchecks;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;
using HotChocolate.AspNetCore;

namespace Demo.DbRefreshManager.WebApi;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var environment = builder.Environment;
        var config = builder.Configuration;

        // Add config providers.
        // Environment.
        config.AddEnvironmentVariables(prefix: AppConstants.EnvironmentPrefix);

        // Доступ к HttpContext через инъекции.
        services.AddHttpContextAccessor();

        // Application.
        services.AddApplicationOptions();
        services.AddApplicationFeatures();

        // Infrastructure.
        services.AddInfrastructure();
        services.AddDatabase(config, environment.IsDevelopment());
        services.AddDatabaseFeatures();

        // Presentation (WebApi).
        services.AddWebApiServices();
        services.AddWebApiOptions();
        services.AddWebApiLogging(environment, config);
        services.AddProblemDetails();
        services.AddRestVersioning();
        services.AddGraphQL();
        services.AddQuartzJobs(environment);

        // Api auth.
        services.AddCookieAuthentication(config);
        services.AddAuthorization();

        // Healthcheck services.
        services.AddHealthChecks().AddCheck<EfCheck>(nameof(EfCheck));

        if (!environment.IsProduction())
        {
            services.AddVersionedOpenApiDocs();
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!environment.IsProduction())
        {
            app.MapOpenApiWithScalar();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseWebSockets();

        app.UseDefaultFiles();
        app.MapStaticAssets();

        app.MapVersionedApiEndpoints();

        app.MapGraphQL().WithOptions(new()
        {
            AllowedGetOperations = AllowedGetOperations.Query,
            EnableSchemaRequests = !environment.IsProduction(),
            Tool = { Enable = !environment.IsProduction() }
        });

        app.MapHealthChecks("healthz", new()
        {
            ResponseWriter = HealthcheckResponseWriters.WriteTextResponse
        });

        app.MapGet("env", () => environment.EnvironmentName);

        // Fallback to index.html for SPA routes.
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
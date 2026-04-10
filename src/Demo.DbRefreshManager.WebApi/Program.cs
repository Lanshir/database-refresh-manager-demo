using Demo.DbRefreshManager.Dal.Extensions;
using Demo.DbRefreshManager.WebApi.Endpoints.Extensions;
using Demo.DbRefreshManager.WebApi.Infrastructure.Constants;
using Demo.DbRefreshManager.WebApi.Infrastructure.Endpoints;
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
        config.AddEnvironmentVariables(prefix: ProjectConstants.EnvironmentPrefix);

        // Healthcheck services.
        services.AddHealthChecks().AddCheck<EfCheck>(nameof(EfCheck));

        services.AddProblemDetails();

        services.AddCookieAuthentication(config);
        services.AddAuthorization();

        // Доступ к HttpContext через инъекции.
        services.AddHttpContextAccessor();

        services.AddProjectCoreDependencies();
        services.AddApiOptions();
        services.AddLoggingServices(environment, config);
        services.AddDatabaseServices(config, environment.IsDevelopment());
        services.AddRestVersioning();
        services.AddGraphQLServices();
        services.AddQuartzJobsServices(environment);

        if (!environment.IsProduction())
        {
            services.AddVersionedOpenApiDocs();
        }

        var app = builder.Build();
        var endpointExceptionsFilter = new EndpointExceptionsFilter();

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

        app.MapApiEndpointGroups();

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
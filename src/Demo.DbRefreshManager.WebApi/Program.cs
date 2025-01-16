using Demo.DbRefreshManager.Dal.Extensions;
using Demo.DbRefreshManager.WebApi.Infrastructure.Constants;
using Demo.DbRefreshManager.WebApi.Infrastructure.Controllers;
using Demo.DbRefreshManager.WebApi.Infrastructure.Extensions;
using Demo.DbRefreshManager.WebApi.Infrastructure.Healthchecks;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

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

        // Add services to the container.
        services.AddControllers(o =>
        {
            // Kebab case controller names transform.
            o.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer()));
            // Controller Exception handling.
            o.Filters.Add<ControllerExceptionFilter>();
        });

        if (!environment.IsProduction())
        {
            // Swagger.
            services.AddSwaggerGeneration();
        }

        // Healthcheck services.
        services.AddHealthChecks().AddCheck<EfCheck>(nameof(EfCheck));

        services.AddCookieAuthentication(config);
        services.AddAuthorization();

        // Доступ к HttpContext через инъекции.
        services.AddHttpContextAccessor();

        services.AddProjectCoreDependencies();
        services.AddLoggingServices(environment, config);
        services.AddDatabaseServices(config, environment.IsDevelopment());
        services.AddRestVersioning();
        services.AddGraphQLServices();
        services.AddQuartzJobsServices(environment);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!environment.IsProduction())
        {
            // Swagger.
            app.UseProjectSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseWebSockets();

        app.UseDefaultFiles();
        app.MapStaticAssets();

        app.MapControllers();

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
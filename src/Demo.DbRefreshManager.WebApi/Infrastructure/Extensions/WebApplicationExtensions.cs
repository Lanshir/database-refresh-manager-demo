using Demo.DbRefreshManager.WebApi.Infrastructure.Static;
using Scalar.AspNetCore;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    extension(WebApplication app)
    {
        /// <summary>
        /// Настройка докуменации OpenApi и ScalarUI.
        /// </summary>
        public WebApplication MapOpenApiWithScalar()
        {
            app.MapOpenApi();

            app.MapScalarApiReference(o => o
                .AddDocuments(SupportedApiVersions.VersionsList.Select(v => $"v{v}"))
                .EnableDarkMode()
                .WithTheme(ScalarTheme.BluePlanet));

            return app;
        }
    }
}

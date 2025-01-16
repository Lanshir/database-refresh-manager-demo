using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Extensions;

/// <summary>
/// Расширения IApplicationBuilder.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Использовать swagger.
    /// </summary>
    /// <param name="routePrefix">Префикс пути к swagger ui.</param>
    public static IApplicationBuilder UseProjectSwaggerUI(
        this IApplicationBuilder app,
        string routePrefix = "swagger")
    {
        var versionProvider = app.ApplicationServices
            .GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.List);
                options.DefaultModelsExpandDepth(-1);

                // Build a swagger endpoint for each discovered API version.
                foreach (var description in versionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());

                    options.RoutePrefix = routePrefix;
                }
            });

        return app;
    }
}
